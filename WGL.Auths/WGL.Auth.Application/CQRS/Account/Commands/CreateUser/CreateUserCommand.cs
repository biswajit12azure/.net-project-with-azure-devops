using AutoMapper;
using MediatR;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Auth.Domain.Entities;
using WGL.Utility.EmailService;
using WGL.Utility.Password;
using WGL.Utility.Wrappers;

namespace WGL.Auth.Application.CQRS.Account.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<Response<string>>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public required string FullName { get; set; }
        public required string CompanyName { get; set; }
        public required string MobileNumber { get; set; }
        public required string EmailAddress { get; set; }
        public required string Password { get; set; }
        public required int PortalId { get; set; }
    }
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Response<string>>
    {
        private readonly IAccountRepositoryAsync _accountRepositoryAsync;
        private readonly IMapper _mapper;
        private readonly IEmailServiceAsync _emailServiceAsync;
        public CreateUserCommandHandler(IAccountRepositoryAsync accountRepositoryAsync, IMapper mapper, IEmailServiceAsync emailServiceAsync)
        {
            _accountRepositoryAsync = accountRepositoryAsync;
            _mapper = mapper;
            _emailServiceAsync = emailServiceAsync;
        }
        public async Task<Response<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var UserDetials = _mapper.Map<ApplicationUser>(new ApplicationUser()
            {
                CompanyName = request.CompanyName,
                MobileNumber = request.MobileNumber,
                FirstName = request.FirstName,
                LastName = request.LastName,
                FullName=request.FullName,
                Password = PasswordEncoder.EncodePasswordToBase64(request.Password.Trim()),
                EmailID = request.EmailAddress,
                CreatedBy = 0,
                PortalId= request.PortalId
            });

            var result = await _accountRepositoryAsync.CreateUserAsync(UserDetials);
            if (result != "EmaildExist")
            {
              await _emailServiceAsync.SendAsync(new EmailRequest() 
                                                    { To = request.EmailAddress, 
                                                      Subject = "Washgas : Welcome",
                                                      Body= "<a href=\"http://localhost:3000/registration/verified?verifyId=" + PasswordEncoder.EncodePasswordToBase64(result) + "\">Verify Email</a>"
              });
                return new Response<string>(result, message: $"New User Created");
            }
            else
            {
                return new Response<string>(result, message: $"Email Already Exist");
            }
        }
    }
}
