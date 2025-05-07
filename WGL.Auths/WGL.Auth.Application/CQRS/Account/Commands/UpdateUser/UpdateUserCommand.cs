using MediatR;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Auth.Domain.Entities;
using WGL.Utility.Wrappers;

namespace WGL.Auth.Application.CQRS.Account.Commands.UpdateUser
{
    public class UpdateUserCommand:IRequest<Response<GetUpdatedUserDetails>>
    {
        public string UserId{ get; set; }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Response<GetUpdatedUserDetails>>
    {
        private readonly IAccountRepositoryAsync _repository;
        public UpdateUserCommandHandler(IAccountRepositoryAsync repository)
        {
            _repository = repository;
        }
        public async Task<Response<GetUpdatedUserDetails>> Handle(UpdateUserCommand updateUser, CancellationToken cancellationToken)
        {
            var result = await _repository.GetUpdatedUserDetailsIdAsync(updateUser.UserId);
            if (result !=null) {
                return new Response<GetUpdatedUserDetails>(result, message: "Updated Successfully");
            }
            else {
            return new Response<GetUpdatedUserDetails>(result, message: "Update Failed");
            }
        }
    }
}
