using MediatR;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Utility.EmailService;
using WGL.Utility.Password;
using WGL.Utility.Wrappers;

namespace WGL.Auth.Application.CQRS.Account.Commands.ResendEmail
{
    public class ResendEmailCommand : IRequest<Response<string>>
    {
        public required string Emailaddress { get; set; }
        public required int UserId { get; set; }
    }
    public class ResendEmailCommandHandler : IRequestHandler<ResendEmailCommand, Response<string>>
    {
        private readonly IAccountRepositoryAsync _repository;
        private readonly IEmailServiceAsync _emailServiceAsync;
        public ResendEmailCommandHandler(IAccountRepositoryAsync repository, IEmailServiceAsync emailServiceAsync)
        {
            _repository = repository;
            _emailServiceAsync = emailServiceAsync;
        }
        public async Task<Response<string>> Handle(ResendEmailCommand updateUser, CancellationToken cancellationToken)
        {
            //var result = await _repository.ResendEmailVerification(updateUser.UserId);
            await _emailServiceAsync.SendAsync(new EmailRequest()
            {
                To = updateUser.Emailaddress,
                Subject = "Washgas : Welcome",
                Body = "<a href=\"http://localhost:3000/registration/verified?verifyId=" + PasswordEncoder.EncodePasswordToBase64(Convert.ToString(updateUser.UserId)) + "\">Verify Email</a>"
            });
            return new Response<string>("Email Sent", message: $"Re-verify Email Sent");
         
        }
    }

}

