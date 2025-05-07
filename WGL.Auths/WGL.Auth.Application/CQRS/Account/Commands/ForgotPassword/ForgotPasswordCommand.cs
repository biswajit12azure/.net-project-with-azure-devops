using MediatR;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Utility.EmailService;
using WGL.Utility.Exceptions;
using WGL.Utility.Password;
using WGL.Utility.Wrappers;

namespace WGL.Auth.Application.CQRS.Account.Commands.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest<Response<string>>
    {
        public required string EmailAddress { get; set; }
    }
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Response<string>>
    {
        private readonly IAccountRepositoryAsync _accountRepositoryAsync;
        private readonly IEmailServiceAsync _emailServiceAsync;
        public ForgotPasswordCommandHandler(IAccountRepositoryAsync accountRepositoryAsync, IEmailServiceAsync emailServiceAsync)
        {
            _accountRepositoryAsync = accountRepositoryAsync;
            _emailServiceAsync = emailServiceAsync;
        }
        public async Task<Response<string>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _accountRepositoryAsync.ForgotPasswordAsync(request.EmailAddress);
            if (string.IsNullOrEmpty(result))
            {
                throw new ApiException($"Invalid {result} Address.");
            }
            await _emailServiceAsync.SendAsync(new EmailRequest()
            {
                To = request.EmailAddress,
                Subject = "Washgas : Forgot Password",
                Body = "<a href=\"http://localhost:3000/forgotpassword/verified?verifyId=" + PasswordEncoder.EncodePasswordToBase64(result) + "\">Forgot Password</a>"
            });
            return new Response<string>(result, message: $"Password reset link sent to your provided email address");           
        }
    }
}
