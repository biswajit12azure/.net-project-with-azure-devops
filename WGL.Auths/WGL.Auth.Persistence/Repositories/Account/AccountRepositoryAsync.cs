using Dapper;
using LazyCache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Serilog;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using WGL.Auth.Application.DTOs.Account;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Auth.Domain.Constants;
using WGL.Auth.Domain.Entities;
using WGL.Auth.Domain.Settings;
using WGL.Utility.DBContext;
using WGL.Utility.Exceptions;
using WGL.Utility.Wrappers;
using WGL.Utility.Helper;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using WGL.Utility.Password;
using WGL.Auth.Application.CQRS.Account.Queries.SupplierDiversityGetUsers;
using WGL.Auth.Application.CQRS.Account.Queries.MapCentreGetUsers;
using WGL.Auth.Application.DTOs.Master;
using System.Collections.Concurrent;
using WGL.Utility.BlobFiles.FileInterface;
using WGL.Utility.BlobFiles.FileRepository;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;

namespace WGL.Auth.Persistence.Repositories.Account
{
    public class AccountRepositoryAsync(IOptions<AppSettings> appSettings, IOptions<JWTSettings> jwtSettings, ICacheProvider cacheProvider, DBContext dapperContext) :  IAccountRepositoryAsync
    {
        private readonly AppSettings _appSettings = appSettings.Value;
        private readonly JWTSettings _jwtSettings = jwtSettings.Value;
        private readonly ICacheProvider _cacheProvider = cacheProvider;
        private readonly DBContext _dapperContext = dapperContext;
        private readonly IDbConnection _dbconnection = dapperContext.CreateConnection();
        ConcurrentDictionary<string, (string Otp, DateTime Expiration)> _store = new();

        private async Task<JwtSecurityToken> GenerateJWToken(Userdetails user)
        {
            string ipAddress = IpHelper.GetIpAddress();
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.email),
                new Claim("uid", user.email),
                new Claim("ip", ipAddress)
            };
            //.Union(userClaims)
            //.Union(roleClaims);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        public async Task<GenerateTokenResponse> GenerateTokenQuery(/*string AppKey, string SecretKey*/)
        {
            if (!_cacheProvider.TryGetValue(Constants.getOneLoginToken, out GenerateTokenResponse getToken))
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, _appSettings.Endpoint + Constants.getTokenMethod);
                request.Headers.Add(Constants.Authorization, _appSettings.EncryptedCreds);
                var collection = new List<KeyValuePair<string, string>>();
                collection.Add(new(Constants.grantType, _appSettings.GrantType));
                collection.Add(new(Constants.userName, _appSettings.AppId));
                collection.Add(new(Constants.password, _appSettings.SecretKey));
                var content = new FormUrlEncodedContent(collection);
                request.Content = content;
                var response = await client.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    Log.Error("Error occured while getting Token => {@StatusCode},{@EndPoint},{@UserName},{@Password}",
                                                        response.StatusCode,
                                                        _appSettings.Endpoint + Constants.getTokenMethod,
                                                        _appSettings.AppId, _appSettings.SecretKey);
                    throw new ApiException($"{response.Content.ReadAsStringAsync().Result}");

                }
                getToken = JsonSerializer.Deserialize<GenerateTokenResponse>(response.Content.ReadAsStringAsync().Result);
                if (getToken != null && getToken.access_token != null)
                {
                    var cacheEntryOption = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddSeconds(getToken.expires_in),
                        SlidingExpiration = TimeSpan.FromSeconds(getToken.expires_in),
                        Size = 1024
                    };
                    _cacheProvider.Set(Constants.getOneLoginToken, getToken, cacheEntryOption);
                }
            }
            Log.ForContext("UniqueId", Guid.NewGuid().ToString("N").Substring(0, 20).ToUpper())
               .Information("OneLogin-GenerateTokenQuery: Received Token => {@Token}", getToken.access_token);
            return getToken;
        }

        public async Task<Response<AuthenticationResponse>> GenerateUserTokenQuery(string UserName, string Password)
        {
            AuthenticationResponse userToken = new();
            var userDict = new Dictionary<string, Userdetails>();
            var AccessDict = new Dictionary<string, Useraccess>();
            List<Useraccess> lstUserAccesses = [];
            var Params = new DynamicParameters();
            Params.Add("@EmailID", UserName);
            Params.Add("@Password", PasswordEncoder.EncodePasswordToBase64(Password));
            var users = (await _dbconnection.QueryAsync<AuthendicationDBData>("Sp_GetUserPortalRoleMapping", Params, commandType: CommandType.StoredProcedure)).ToList();
            if (users.Count != 0)
            {
                foreach (var user in users)
                {
                    if (!userDict.TryGetValue(user.EmailID, out var CurrentUsers))
                    {
                        CurrentUsers = new Userdetails()
                        {
                            email = user.EmailID,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            id = user.UserID,
                            Status = user.Status,
                            isVerified = user.IsuserActive
                        };
                        userDict.Add(user.EmailID, CurrentUsers);
                        userToken.UserDetails = CurrentUsers;
                    }
                    if (!AccessDict.TryGetValue(user.PortalName, out var CurrentAccess))
                    {
                        List<Roleaccess> lstroleaccesses = users.Where(x => x.PortalName.Equals(user.PortalName))
                                                                .Select(u => new Roleaccess
                                                                {
                                                                    AccessName = u.AccessName,
                                                                    IsActive = u.IsAccessActive,
                                                                    AccessKey = u.AccessKey
                                                                }).ToList();
                        AccessDict.Add(user.PortalName, CurrentAccess);
                        CurrentAccess = new Useraccess()
                        {
                            PortalId = user.PortalId,
                            PortalKey = user.PortalKey,
                            PortalName = user.PortalName,
                            RoleId = user.RoleId,
                            Role = user.RoleName,
                            IsMandateDone = user.IsMandateDone,
                            RoleAccess = lstroleaccesses
                        };

                        lstUserAccesses.Add(CurrentAccess);
                        userToken.UserAccess = lstUserAccesses;
                    }
                }
                JwtSecurityToken jwtSecurityToken = await GenerateJWToken(userToken.UserDetails);
                userToken.UserDetails.jwToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                userToken.UserDetails.tokenExpiry = DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes);
                return new Response<AuthenticationResponse>(userToken, $"Authenticated {userToken.UserDetails.email}");
            }
            return new Response<AuthenticationResponse>(userToken);

        }

        public Task<bool> IsDuplicateUser(string EmailAddress)
        {
            throw new NotImplementedException();
        }

        public async Task<string> CreateUserAsync(ApplicationUser applicationUser)
        {
            DynamicParameters Params = new();
            Params.Add("@FirstName", applicationUser.FirstName);
            Params.Add("@LastName", applicationUser.LastName);
            Params.Add("@FullName", applicationUser.FullName);
            Params.Add("@CompanyName", applicationUser.CompanyName);
            Params.Add("@MobileNumber", applicationUser.MobileNumber);
            Params.Add("@EmailID", applicationUser.EmailID);
            Params.Add("@Password", PasswordEncoder.EncodePasswordToBase64(applicationUser.Password)); //Convert.FromBase64String(input);
            Params.Add("@CreatedBy", applicationUser.CreatedBy);
            Params.Add("@StatusID", 1);
            Params.Add("@StatementType", "Insert");
            Params.Add("@PortalId", applicationUser.PortalId);
            Params.Add("@EmailSentStartTime", DateTime.UtcNow);
            Params.Add("@EmailSentEndTime", DateTime.UtcNow.AddHours(24));
            Params.Add("@FailedAttemptCount", 0);
            Params.Add("@IsAccountLock", 0);
            //Params.Add("@ReturnUserValue", null,DbType.String, ParameterDirection.Output,100);
            var result= await _dbconnection.QueryAsync<string>("Sp_User_CRUD", Params, commandType: CommandType.StoredProcedure);
            return result.FirstOrDefault();
        }


        public Task<IEnumerator<ApplicationUser>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ApplicationUser> GetUserByIdAsync(int UserId)
        {
            throw new NotImplementedException();
        }


        public async Task<GetUpdatedUserDetails?> GetUpdatedUserDetailsIdAsync(string UserId)
        {
            DynamicParameters Params = new();
            Params.Add("@UserID", PasswordEncoder.DecodeFrom64(Convert.ToString(UserId)));
            return (await _dbconnection.QueryAsync<GetUpdatedUserDetails?>("Sp_UpdateUserStatusById", Params, commandType: CommandType.StoredProcedure)).FirstOrDefault();
        }

        public async Task<int> InsertAdditionalMCUserDataAsync(AdditionalMC user)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@AdditionalID", user.AdditionalID);
            parameters.Add("@UserID", user.UserID);
            parameters.Add("@FullName", user.FullName);
            parameters.Add("@AlternateEmail", user.AlternateEmail);
            parameters.Add("@DLState", user.DLState);
            parameters.Add("@DLNumber", user.DLNumber);
            parameters.Add("@CompanyName", user.CompanyName);
            parameters.Add("@TaxIdentificationNumber", user.TaxIdentificationNumber);
            parameters.Add("@StreetAddress1", user.CompanyStreetAddress1);
            parameters.Add("@StreetAddress2", user.CompanyStreetAddress2);
            parameters.Add("@City", user.CompanyCity);
            parameters.Add("@State", user.CompanyState);
            parameters.Add("@ZipCode", user.CompanyZipCode);
            parameters.Add("@ContactName", user.CompanyContactName);
            parameters.Add("@ContactTelephone", user.CompanyContactTelephone);
            parameters.Add("@ContactEmailAddress", user.CompanyContactEmailAddress);
            parameters.Add("@AuthorizedWGLContact", user.AuthorizedWGLContact);
            parameters.Add("@HomeStreetAddress1", user.HomeStreetAddress1);
            parameters.Add("@HomeStreetAddress2", user.HomeStreetAddress2);
            parameters.Add("@HomeCity", user.HomeCity);
            parameters.Add("@HomeState", user.HomeState);
            parameters.Add("@HomeZipCode", user.HomeZipCode);
            parameters.Add("@StatementType", "Insert");

            //return await _dbconnection.ExecuteScalarAsync<int>("Sp_UserAdditionalMC_CRUD", parameters, commandType: CommandType.StoredProcedure);
            var result = await _dbconnection.QueryAsync<string>("Sp_UserAdditionalMC_CRUD", parameters, commandType: CommandType.StoredProcedure);
            string additionalstr = result.FirstOrDefault();
            int additionalint = 0;
            if (additionalstr != "User MC Record already exist")
            {
                additionalint = Convert.ToInt32(additionalstr);
            }
            return additionalint;
        }
        public async Task<int> InsertAdditionalSDUserDataAsync(AdditionalSD user)
        {
            var parameters = new DynamicParameters();
            parameters.Add("AdditionalID", user.AdditionalID);
            parameters.Add("@UserID", user.UserID);
            parameters.Add("@CompanyName", user.CompanyName);
            parameters.Add("@ContactPerson", user.ContactPerson);
            parameters.Add("@Title", user.Title);
            parameters.Add("@Street", user.Street);
            parameters.Add("@City", user.City);
            parameters.Add("@State", user.State);
            parameters.Add("@CompanyWebsite", user.CompanyWebsite);
            parameters.Add("@Email", user.Email);
            parameters.Add("@ZipCode", user.ZipCode);
            parameters.Add("@PhoneNumber", user.PhoneNumber);
            parameters.Add("@Fax", user.Fax);
            parameters.Add("@CellPhone", user.CellPhone);
            parameters.Add("@CategoryID", user.CategoryID);
            parameters.Add("@ClassificationID", user.ClassificationID);
            parameters.Add("@ServicesProductsProvided", user.ServicesProductsProvided);
            parameters.Add("@ExpiryDate", user.ExpiryDate);
            parameters.Add("@AgencyID", user.AgencyID);
            parameters.Add("@AgencyStateID", user.AgencyStateID);
            parameters.Add("@StatementType", "Insert");

            var result = await _dbconnection.QueryAsync<string>("Sp_UserAdditionalSD_CRUD", parameters, commandType: CommandType.StoredProcedure);
            string additionalstr = result.FirstOrDefault();
            int additionalint = 0;
            if (additionalstr != "User SD Record already exist")
            {
                additionalint = Convert.ToInt32(additionalstr);
            }
            return additionalint;
         
        }

        public async Task UploadUserDocumentsAsync(List<Document> fileData,int additionalID)
        {
            foreach (var file in fileData)
            {
                var documentParameters = new DynamicParameters();
                documentParameters.Add("@ID", file.ID);
                documentParameters.Add("@AdditionalID", additionalID);
                documentParameters.Add("@DocumentTypeID", file.DocumentTypeID);
                documentParameters.Add("@FileName", file.FileName);
                documentParameters.Add("@Format", file.Format);
                documentParameters.Add("@Size", file.Size);
                documentParameters.Add("@URL", file.Url);
                documentParameters.Add("@PortalKey", file.PortalKey);
                documentParameters.Add("@StatementType", "Insert");

                await _dbconnection.ExecuteAsync("Sp_DocumentDetails_CRUD", documentParameters, commandType: CommandType.StoredProcedure);
            }

        }

        public async Task<List<DocumentTypeResponse>> GetDocumentType(string PortalKey)
        {
            var documentDetails = _dbconnection.Query<(int DocumentID, string DocumentName)>(
             "SELECT DocumentID, DocumentName FROM DocumentType WHERE PortalKey=" + "'" + PortalKey + "'"
               ).ToDictionary(x => x.DocumentID, x => x.DocumentName);

            List<DocumentTypeResponse> lst = new List<DocumentTypeResponse>();
            foreach (KeyValuePair<int, string> pair in documentDetails)

            {
                DocumentTypeResponse obj = new DocumentTypeResponse();
                obj.DocumentID = pair.Key;
                obj.DocumentName = pair.Value;
                lst.Add(obj);
            }
            return lst;
        }
        public async Task<List<BusinessCategoryResponse>> GetBusinessCategory()
        {
            var categoryDetails = _dbconnection.Query<(int CategoryID, string CategoryName)>(
             "SELECT CategoryID, CategoryName FROM BusinessCategorySD"
               ).ToDictionary(x => x.CategoryID, x => x.CategoryName);

            List<BusinessCategoryResponse> lst = new List<BusinessCategoryResponse>();
            foreach (KeyValuePair<int, string> pair in categoryDetails)

            {
                BusinessCategoryResponse obj = new BusinessCategoryResponse();
                obj.CategoryID = pair.Key;
                obj.CategoryName = pair.Value;
                lst.Add(obj);
            }
            return lst;
        }
        public async Task<List<ClassificationResponse>> GetClassification()
        {
            var categoryDetails = _dbconnection.Query<(int CategoryID, string CategoryName)>(
             "SELECT ClassificationID, ClassificationName FROM ClassificationSD"
               ).ToDictionary(x => x.CategoryID, x => x.CategoryName);

            List<ClassificationResponse> lst = new List<ClassificationResponse>();
            foreach (KeyValuePair<int, string> pair in categoryDetails)

            {
                ClassificationResponse obj = new ClassificationResponse();
                obj.ClassificationID = pair.Key;
                obj.ClassificationName = pair.Value;
                lst.Add(obj);
            }
            return lst;
        }

        public async Task<AdditionalSDDTO> GetSupplierDiversityUserByIdAsync(GetUserSupplierDiversityQuery supplierDiversityUser)
        {
            IBlobStorageRepository _blobStorage = new BlobStorageRepository();

            DynamicParameters Params = new();
            Params.Add("@UserID", supplierDiversityUser.UserId);
            Params.Add("@StatementType", "Select");
            var res= (await _dbconnection.QueryAsync<AdditionalSDDTO>("Sp_UserAdditionalSD_CRUD", Params, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            DynamicParameters Params1 = new();
            Params1.Add("@AdditionalID", res.AdditionalID);
            Params1.Add("@PortalKey", "SD");
            res.FileData = (await _dbconnection.QueryAsync<DocumentDTO>("Sp_GetDocumentListbyId", Params1, commandType: CommandType.StoredProcedure)).ToList();
            //foreach (var docFile in res.FileData)
            //{
            //    var fileName = await _blobStorage.DownloadFileAsync(docFile.FileName);
            //    var base64File = Convert.ToBase64String(fileName);
            //    docFile.File = base64File;
            //}
            foreach (var docFile in res.FileData)
            {
                var fileName = await _blobStorage.DownloadFileAsync(docFile.Url);
                var base64File = Convert.ToBase64String(fileName);
                docFile.File = base64File;

            }
            DynamicParameters Params2 = new();
            Params2.Add("@PortalKey", "SD");
            res.DocumentData= (await _dbconnection.QueryAsync<DocumentTypeDTO>("SP_MasterDocumentTypeByPortalKey", Params2, commandType: CommandType.StoredProcedure)).ToList();
            DynamicParameters Params3 = new();
            res.State1= (await _dbconnection.QueryAsync<StateDTO>("SP_GetMasterState", Params3, commandType: CommandType.StoredProcedure)).ToList();
            DynamicParameters Params4 = new();
            res.BusinessCategory = (await _dbconnection.QueryAsync<BusinessCategoryDTO>("SP_GetMasterBusinessCategory", Params4, commandType: CommandType.StoredProcedure)).ToList();
            DynamicParameters Params5 = new();
            res.Classification = (await _dbconnection.QueryAsync<ClassificationDTO>("SP_GetMasterClassification", Params5, commandType: CommandType.StoredProcedure)).ToList();
            DynamicParameters Params6 = new();
            Params6.Add("@StatementType", "Select");
            res.Agency = (await _dbconnection.QueryAsync<AgencyDTO>("Sp_Agency_CRUD", Params6, commandType: CommandType.StoredProcedure)).ToList();
            return res;
        }

        public async Task<UserResponseMCDTO> GetMapCenterUserByIdAsync(GetUserMapCenterQuery mapCenterUser)
        {
            IBlobStorageRepository _blobStorage = new BlobStorageRepository();
            UserResponseMCDTO usrmcdto = new UserResponseMCDTO();
            DynamicParameters Params = new();
            DynamicParameters Params1 = new();
            DynamicParameters Params2 = new();
            DynamicParameters Params3 = new();
            Params.Add("@UserID", mapCenterUser.UserId);
            Params.Add("@StatementType", "Select");
            var result = (await _dbconnection.QueryAsync("Sp_UserAdditionalMC_CRUD", Params, commandType: CommandType.StoredProcedure)).ToList();
           // usrmcdto = result.FirstOrDefault();
            int? userAdditnalId = 0;
            if (result.Count > 0)
            {
                foreach (var user in result)
                {
                    UserResponseMCDTO usrmcdto1 = new UserResponseMCDTO()
                    {
                        EmailID = user.EmailID,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserID = user.UserID,
                        StatusID = user.Status,
                        IsActive = user.IsActive,
                        CompanyName = user.CompanyName,
                        MobileNumber = user.MobileNumber,
                        AlternateEmail = user.ALternateEmail,
                        HomeStreetAddress1 = user.HomeStreetAddress1,
                        HomeStreetAddress2 = user.HomeStreetAddress2,
                        HomeCity = user.HomeCity,
                        HomeState = user.HomeState,
                        HomeZipCode = user.HomeZip,
                        State = user.State,
                        AuthorizedWGLContact = user.AuthorizedWGLContact,
                        CompanyCity = user.CompanyCity,
                        CompanyContactEmailAddress = user.CompanyContactEmailAddress,
                        CompanyContactTelephone = user.CompanyContactTelephone,
                        CompanyContactName = user.CompanyContactName,
                        CompanyState = user.CompanyState,
                        CompanyStreetAddress1 = user.CompanyStreetAddress1,
                        CompanyStreetAddress2 = user.CompanyStreetAddress2,
                        CompanyZipCode = user.CompanyZipCode,
                        DLNumber = user.DLNumber,
                        DLState = user.DLState,
                        FileData = user.FileData,
                        FullName = user.FullName,
                        ID = user.ID,
                        TaxIdentificationNumber = user.TaxIdentificationNumber,
                        AdditionalID = user.AdditionalID,
                    };
                    usrmcdto = usrmcdto1;
                }
            }
            userAdditnalId = usrmcdto.AdditionalID;
                Params1.Add("@AdditionalID", userAdditnalId);
                Params1.Add("@PortalKey","MC");
                var Docresult = (await _dbconnection.QueryAsync<DocumentDTO>("Sp_GetDocumentListbyId", Params1, commandType: CommandType.StoredProcedure)).ToList();
                usrmcdto.FileData = Docresult;
                foreach (var docFile in Docresult)
                {
                    var fileName = await _blobStorage.DownloadFileAsync(docFile.FileName);
                    var base64File = Convert.ToBase64String(fileName);
                    docFile.File = base64File;
                    
                }
                Params2.Add("@@PortalKey", "MC");
                usrmcdto.DocumentData = (await _dbconnection.QueryAsync<DocumentTypeDTO>("SP_MasterDocumentTypeByPortalKey", Params2, commandType: CommandType.StoredProcedure)).ToList();

                usrmcdto.State = (await _dbconnection.QueryAsync<StateDTO>("SP_GetMasterState", Params3, commandType: CommandType.StoredProcedure)).ToList();
            return usrmcdto;
        }

        public async Task<UserDetailsResponse> GetUserDetailsByID(int userID,int portalID)
        {
            DynamicParameters Params = new();
            Params.Add("@UserID", userID);
            Params.Add("@PortalID", portalID);
            var res = (await _dbconnection.QueryAsync<UserDetailsResponse>("Sp_GetUserDetailsByID", Params, commandType: CommandType.StoredProcedure)).FirstOrDefault();
            return res;
        }

        public async Task<string> GenerateOTPAsync(string Email)
        {
            var random = new Random();
            string OTP = random.Next(100000, 999999).ToString();
            var cacheEntryOption = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(_appSettings.OTPDuration),
                SlidingExpiration = TimeSpan.FromMinutes(_appSettings.OTPDuration),
                Size = 1
            };
            _cacheProvider.Set(Email, OTP, cacheEntryOption);
            return await Task.FromResult(OTP);
        }

        public async Task<bool> VerifyOTPAsync(string Email, string OTP)
        {
            if(_cacheProvider.TryGetValue(Email, out string otp))
            {
                if (otp == OTP)
                {
                    return await Task.FromResult(true);
                }
            }
            return await Task.FromResult(false);
        }

        public Task<int> UpdateUserAsync(ApplicationUser applicationUser)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteUserAsync(int UserId)
        {
            throw new NotImplementedException();
        }

        public Task<GetUpdatedUserDetails> GetUpdatedUserDetailsIdAsync(int UserId)
        {
            throw new NotImplementedException();
        }

        public Task<string> ResendEmailVerification(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> ForgotPasswordAsync(string Email)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@EmailID", Email);
            dynamicParameters.Add("@StatementType", "Select");
            var UserEmail = await _dbconnection.QueryAsync<ApplicationUser>("Sp_User_CRUD", dynamicParameters, commandType: CommandType.StoredProcedure);
            var user = UserEmail.FirstOrDefault();
            if (user != null)
            {
                return Convert.ToString(user.UserId) ?? string.Empty;
            }
            return string.Empty;
        }

        public async Task<int> ResetPasswordAsync(int UserId, string Password)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@Password", PasswordEncoder.EncodePasswordToBase64(Password));
            dynamicParameters.Add("@Id", UserId);
            dynamicParameters.Add("@StatementType", "Update");
            return await _dbconnection.ExecuteAsync("Sp_User_CRUD", dynamicParameters, commandType: CommandType.StoredProcedure);
        }
        public async Task<List<UserProfileResponse>> GetUserProfileByPortalID(int PortalID)
        {

            DynamicParameters dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("@PortalID", PortalID);
                        
            var userList = (await _dbconnection.QueryAsync<UserRequestParms>("Sp_GetUserListByPortalID", dynamicParameters,commandType: CommandType.StoredProcedure)).ToList();

            var agencies = (await _dbconnection.QueryAsync<AgencyRequestParms>("Sp_Agency_CRUD",new { StatementType = "Select" },commandType: CommandType.StoredProcedure)).ToList();

            var roles = (await _dbconnection.QueryAsync<RoleRequestParms>("Sp_Role_CRUD",new { StatementType = "Select" },commandType: CommandType.StoredProcedure)).ToList();

            var marketers = (await _dbconnection.QueryAsync<MarketerRequestParms>("Sp_Marketer_CRUD",new { StatementType = "Select" },commandType: CommandType.StoredProcedure)).ToList();

            var responseList = new List<UserProfileResponse>();

            responseList.Add(new UserProfileResponse
            {
                User = userList,
                Agency = agencies,
                Roles = roles,
                Marketer = marketers
            });

            return responseList;
        }
    }
}
