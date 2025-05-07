using MediatR;
using Microsoft.AspNetCore.Mvc;
using WGL.Auth.Application.CQRS.Account.Commands.CreateUser;
using WGL.Auth.Application.CQRS.Account.Commands.FileUpload;
using WGL.Auth.Application.CQRS.Account.Commands.ForgotPassword;
using WGL.Auth.Application.CQRS.Account.Commands.GenerateOTP;
using WGL.Auth.Application.CQRS.Account.Commands.ResendEmail;
using WGL.Auth.Application.CQRS.Account.Commands.ResetPassword;
using WGL.Auth.Application.CQRS.Account.Commands.UpdateUser;
using WGL.Auth.Application.CQRS.Account.Commands.ValidateOTP;
using WGL.Auth.Application.CQRS.Account.Queries.BusinessCategory;
using WGL.Auth.Application.CQRS.Account.Queries.Classification;
using WGL.Auth.Application.CQRS.Account.Queries.DocumentType;
using WGL.Auth.Application.CQRS.Account.Queries.GetUserAuthToken;
using WGL.Auth.Application.CQRS.Account.Queries.GetUserLoginToken;
using WGL.Auth.Application.CQRS.Account.Queries.GetUserProfileByPortalID;
using WGL.Auth.Application.CQRS.Account.Queries.MapCentreGetUsers;
using WGL.Auth.Application.CQRS.Account.Queries.SupplierDiversityGetUsers;
using WGL.Auth.Application.CQRS.Account.Queries.UserDetails;
using WGL.Auth.Application.DTOs.Account;
using WGL.Auth.Controllers.BaseController;

namespace WGL.Auth.Controllers.Account
{
    public class AccountController : BaseApiController
    {
        //[HttpGet("GenerateToken")]
        //public async Task<IActionResult> Get(/*string AppId, string SecretKey*/)
        //{
        //    return Ok(await Mediator.Send(new GenerateTokenQuery() /*{ AppId= AppId, SecretKey = SecretKey }*/));
        //}


        //Session Login Token
        [HttpGet("GetUserLoginToken")]
        public async Task<IActionResult> GetUserLoginToken(string UserName, string Password)
        {
            return Ok(await Mediator.Send(new GenerateUserLoginTokenQuery() { UserName = UserName, Password = Password }));
        }

        // POST api/
        [HttpPost("Authenticate")]
        public async Task<IActionResult> AuthenticateAsync(AuthenticationRequest request)
        {
            //_logger.LogInformation("User:" + request.Email + " Authenticated @ " + DateTime.Now);
            return Ok(await Mediator.Send(new GetUserAuthTokenQuery { UserName = request.Email, Password = request.Password }));
        }
        // POST api/
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(CreateUserCommand userCommand)
        {
            var origin = Request.Headers.Origin;
            return Ok(await Mediator.Send(userCommand));
        }
        [HttpPost("Register-MC")]
        public async Task<IActionResult> RegisterAsync(FileUploadMCUserAdditionalDataCommand userCommand)
        {
            var origin = Request.Headers.Origin;
            return Ok(await Mediator.Send(userCommand));
        }

        [HttpGet("GetRegisterMapCentreAsync/{UserId}")]
        public async Task<IActionResult> GetRegisterMapCentreAsync(int UserId)
        {
            var result = await Mediator.Send(new GetUserMapCenterQuery { UserId = UserId });
            return Ok(result);
        }

        [HttpPost("Register-SD")]
        public async Task<IActionResult> RegisterAsync(FileUploadSDUserAdditionalDataCommand userCommand)
        {
            var origin = Request.Headers.Origin;
            return Ok(await Mediator.Send(userCommand));
        }

        [HttpGet("GetRegisterSupplierDiversityAsync/{userId}")]
        public async Task<IActionResult> GetRegisterSupplierDiversityAsync(int userId)
        {
            var result = await Mediator.Send(new GetUserSupplierDiversityQuery { UserId= userId });
            return Ok(result);
        }
        [HttpGet("GetUserDetailsByIDAsync")]
        public async Task<IActionResult> GetUserDetailsByIDAsync(int userId,int portalID)
        {
            var result = await Mediator.Send(new GetUserDetailsByIDQuery(userId, portalID) { UserID = userId,PortalID=portalID });
            return Ok(result);
        }

        [HttpPost("GenerateOtp/{EmailAddress}")]
        public async Task<IActionResult> GenerateOtpAsync(string EmailAddress)
        {
            return Ok(await Mediator.Send(new GenerateOTPCommand { EmailAddress = EmailAddress }));
        }
        [HttpPost("ValidateOtp")]
        public async Task<IActionResult> ValidateOtpAsync(ValidateOTPCommand validateOTPCommand)
        {
            return Ok(await Mediator.Send(validateOTPCommand));
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordCommand resetPassword)
        {
            return Ok(await Mediator.Send(resetPassword));
            //var abc = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> forgotPasswordAsync(string EmailAddress)
        {
            return Ok(await Mediator.Send(new ForgotPasswordCommand { EmailAddress = EmailAddress }));
        }
        // POST api/
        [HttpPut]
        public void put([FromBody] string value)
        {

        }
        // POST api/
        [HttpPatch]
        public void Patch([FromBody] string value)
        { 

        }
        // POST api/
        [HttpDelete]
        public void Delete([FromBody] string value)
        {

        }

        [HttpGet("VerifiedEmailByUser/{UserId}")]
        public async Task<IActionResult> VerifiedEmailByUserAsync(string UserId)
        {
            return Ok(await Mediator.Send(new UpdateUserCommand { UserId = UserId }));
        }

        //Get Document Type Master by PortalKey
        [HttpGet("GetDocumentType")]
        public async Task<IActionResult> GetDocumentType(string PortalKey)
        {
            return Ok(await Mediator.Send(new GetDocumentTypeQuery(PortalKey)));
        }

        //Get Business Category Master for Supply Diversity 
        [HttpGet("GetBusinessCategory")]
        public async Task<IActionResult> GetBusinessCategory()
        {
            return Ok(await Mediator.Send(new GetBusinessCategoryQuery()));
        }

        //Get Classification Master for Supplier Diversity
        [HttpGet("GetClassification")]
        public async Task<IActionResult> GetClassification()
        {
            return Ok(await Mediator.Send(new GetClassificationQuery()));
        }

        [HttpGet("ResendEmailVerification")]
        public async Task<IActionResult> ResendEmailVerification(string EmailAddress, int UserId)
        {
            var result = await Mediator.Send(new  ResendEmailCommand{Emailaddress= EmailAddress,UserId=UserId } );
            return Ok(result);
        }
        [HttpGet("GetUserProfileByPortalID/{portalID}")]
        public async Task<IActionResult> GetUserProfileByPortalID(int portalID)
        {
            var result = await Mediator.Send(new GetUserProfileByPortalIDHandler { PortalID = portalID });
            return Ok(result);
        }
    }
}
