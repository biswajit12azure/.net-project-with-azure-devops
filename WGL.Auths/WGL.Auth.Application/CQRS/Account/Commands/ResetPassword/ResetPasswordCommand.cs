using MediatR;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Utility.Wrappers;

namespace WGL.Auth.Application.CQRS.Account.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest<Response<int>>
    {
        public required int UserId { get; set; }
        public required string Password { get; set; }
        public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Response<int>>
        {
            private readonly IAccountRepositoryAsync _repository;
            public ResetPasswordCommandHandler(IAccountRepositoryAsync repository)
            {
                _repository = repository;
            }
            public async Task<Response<int>> Handle(ResetPasswordCommand resetPassword, CancellationToken cancellationToken)
            {
                var result = await _repository.ResetPasswordAsync(resetPassword.UserId, resetPassword.Password);
                if (result >0)
                {
                    return new Response<int>(result, message: "Password Reset Successfully");
                }
                else
                {
                    return new Response<int>(result, message: "Failed to Reset Password");
                }
            }
        }
    }
}
