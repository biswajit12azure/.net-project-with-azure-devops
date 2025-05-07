using WGL.Auth.Application.CQRS.Account.Queries.MapCentreGetUsers;
using WGL.Auth.Application.CQRS.Account.Queries.SupplierDiversityGetUsers;
using WGL.Auth.Application.DTOs.Account;
using WGL.Auth.Application.DTOs.Master;
using WGL.Auth.Domain.Entities;
using WGL.Utility.Wrappers;

namespace WGL.Auth.Application.Interfaces.Account
{
    public interface IAccountRepositoryAsync 
    {
        Task<GenerateTokenResponse> GenerateTokenQuery(/*string AppKey, string SecretKey*/);
        Task<Response<AuthenticationResponse>> GenerateUserTokenQuery(string UserName, string Password);
        Task<bool> IsDuplicateUser(string EmailAddress);
        Task<string> CreateUserAsync(ApplicationUser applicationUser);
        Task<int> UpdateUserAsync(ApplicationUser applicationUser);
        Task<int> DeleteUserAsync(int UserId);
        Task<IEnumerator<ApplicationUser>> GetAllUsersAsync();
        Task<ApplicationUser> GetUserByIdAsync(int UserId);
        Task<int> InsertAdditionalMCUserDataAsync(AdditionalMC user);
        Task<int> InsertAdditionalSDUserDataAsync(AdditionalSD user);
        Task UploadUserDocumentsAsync(List<Document> fileData,int additionalID);
        Task<List<DocumentTypeResponse>> GetDocumentType(string portalKey);
        Task<List<BusinessCategoryResponse>> GetBusinessCategory();
        Task<List<ClassificationResponse>> GetClassification();
        //Task<IEnumerator<ApplicationUser>> GetAllUsersAsync();
        //Task<ApplicationUser> GetUserByIdAsync(int UserId);
        Task<GetUpdatedUserDetails> GetUpdatedUserDetailsIdAsync(string UserId);
        Task<AdditionalSDDTO> GetSupplierDiversityUserByIdAsync(GetUserSupplierDiversityQuery supplierDiversityUser);
        Task<GetUpdatedUserDetails> GetUpdatedUserDetailsIdAsync(int UserId);
        Task<UserResponseMCDTO> GetMapCenterUserByIdAsync(GetUserMapCenterQuery mapCenterUser);
        Task<string> GenerateOTPAsync(string Email);
        Task<bool> VerifyOTPAsync(string Email, string OTP);
        Task<string> ResendEmailVerification(string userId);
        Task<string> ForgotPasswordAsync(string Email);
        Task<int> ResetPasswordAsync(int UserId, string Password);
        Task<List<UserProfileResponse>> GetUserProfileByPortalID(int PortalID);
        Task<UserDetailsResponse> GetUserDetailsByID(int UserID,int PortalID);
    }
}
