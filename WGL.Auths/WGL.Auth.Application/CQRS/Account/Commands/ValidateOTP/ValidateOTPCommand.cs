using MediatR;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Utility.Wrappers;

namespace WGL.Auth.Application.CQRS.Account.Commands.ValidateOTP
{
    public class ValidateOTPCommand: IRequest<Response<string>>
    {
        public string? EmailAddress { get; set; }
        public string? OTP { get; set; }
    }
    public class ValidateOTPCommandHandler : IRequestHandler<ValidateOTPCommand, Response<string>>
    {
        private readonly IAccountRepositoryAsync _accountRepositoryAsync;
        public ValidateOTPCommandHandler(IAccountRepositoryAsync accountRepositoryAsync)
        {
            _accountRepositoryAsync = accountRepositoryAsync;
        }
        public async Task<Response<string>> Handle(ValidateOTPCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.EmailAddress) || string.IsNullOrEmpty(request.OTP))
            {
                return new Response<string>("Invalid input", message: "Email address and OTP cannot be null or empty");
            }

            var result = await _accountRepositoryAsync.VerifyOTPAsync(request.EmailAddress, request.OTP);
            if (result== true)
            {
                return new Response<string>("OTP Verified", message: $"OTP Verified");
            }
                
            return new Response<string>("OTP Not Verified", message: $"OTP Not Verified");
           
        }
    }
 }
