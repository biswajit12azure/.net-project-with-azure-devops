using MediatR;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Utility.EmailService;
using WGL.Utility.Wrappers;

namespace WGL.Auth.Application.CQRS.Account.Commands.GenerateOTP
{
    public class GenerateOTPCommand : IRequest<Response<string>>
    {
        public string? EmailAddress { get; set; }
    }
    public class GenerateOTPCommandHandler : IRequestHandler<GenerateOTPCommand, Response<string>>
    {
        private readonly IAccountRepositoryAsync _accountRepositoryAsync;
        private readonly IEmailServiceAsync _emailServiceAsync;
        public GenerateOTPCommandHandler(IAccountRepositoryAsync accountRepositoryAsync, IEmailServiceAsync emailServiceAsync)
        {
            _accountRepositoryAsync = accountRepositoryAsync;
            _emailServiceAsync = emailServiceAsync;
        }
        public async Task<Response<string>> Handle(GenerateOTPCommand request, CancellationToken cancellationToken)
        {
            var result = await _accountRepositoryAsync.GenerateOTPAsync(request.EmailAddress);
            if (!string.IsNullOrEmpty(result))
            {
                await _emailServiceAsync.SendAsync(new EmailRequest()
                {
                    To = request.EmailAddress,
                    Subject = "Washgas : OTP",
                    Body = $"Your OTP code is <h2> " + result + "</h2>"
                });                
            }
            return new Response<string>(result, message: $"OTP sent to your email");
        }
    }
}
