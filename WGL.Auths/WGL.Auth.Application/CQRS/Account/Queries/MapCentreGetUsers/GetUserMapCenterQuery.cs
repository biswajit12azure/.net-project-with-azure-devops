using MediatR;
using WGL.Auth.Application.DTOs.Master;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Utility.Wrappers;

namespace WGL.Auth.Application.CQRS.Account.Queries.MapCentreGetUsers
{
    public class GetUserMapCenterQuery : IRequest<Response<UserResponseMCDTO>>
    {
        public int UserId { get; set; }
    }

    public class GetUserMapCenterQueryHandler : IRequestHandler<GetUserMapCenterQuery, Response<UserResponseMCDTO>>
    {
        private readonly IAccountRepositoryAsync _repository;
        public GetUserMapCenterQueryHandler(IAccountRepositoryAsync repository)
        {
            _repository = repository;
        }
        public async Task<Response<UserResponseMCDTO>> Handle(GetUserMapCenterQuery getUserMapCenter, CancellationToken cancellationToken)
        {
            var result = await _repository.GetMapCenterUserByIdAsync(getUserMapCenter);
            if (result != null)
            {
                return new Response<UserResponseMCDTO>(result, message: "Successfull");
            }
            else
            {
                return new Response<UserResponseMCDTO>(result, message: "Failed");
            }
        }
    }
}
