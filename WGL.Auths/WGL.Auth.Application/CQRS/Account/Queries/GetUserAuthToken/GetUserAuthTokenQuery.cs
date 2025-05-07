using MediatR;
using WGL.Auth.Application.DTOs.Account;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Utility.Exceptions;
using WGL.Utility.Wrappers;

namespace WGL.Auth.Application.CQRS.Account.Queries.GetUserAuthToken
{
    public class GetUserAuthTokenQuery : IRequest<Response<AuthenticationResponse>>
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
    public class GetUserAuthTokenQueryHandler : IRequestHandler<GetUserAuthTokenQuery, Response<AuthenticationResponse>>
    {
        private readonly IAccountRepositoryAsync _accountRepositoryAsync;
        public GetUserAuthTokenQueryHandler(IAccountRepositoryAsync accountRepositoryAsync)
        {
                _accountRepositoryAsync = accountRepositoryAsync;
        }
        public async Task<Response<AuthenticationResponse>> Handle(GetUserAuthTokenQuery request, CancellationToken cancellationToken)
        {
            var user = await _accountRepositoryAsync.GenerateUserTokenQuery(request.UserName, request.Password);
            if (user.Data.UserDetails == null)
            { throw new ApiException($"No Accounts Registered with {request.UserName}."); }
            return new Response<AuthenticationResponse>(user.Data);
        }
    }
}
