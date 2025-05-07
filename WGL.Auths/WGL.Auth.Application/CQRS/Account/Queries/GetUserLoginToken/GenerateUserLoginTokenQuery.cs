using LazyCache;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Utility.Wrappers;
using WGL.Auth.Domain.Constants;
using WGL.Auth.Application.DTOs.Account;

namespace WGL.Auth.Application.CQRS.Account.Queries.GetUserLoginToken
{
    public class GenerateUserLoginTokenQuery : IRequest<Response<AuthenticationResponse>>
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
    }
    public class GenerateUserLoginTokenQueryHandler : IRequestHandler<GenerateUserLoginTokenQuery, Response<AuthenticationResponse>>
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly IAccountRepositoryAsync _accountRepositoryAsync;
        public GenerateUserLoginTokenQueryHandler(ICacheProvider cacheProvider, IAccountRepositoryAsync accountRepositoryAsync)
        {
            _cacheProvider = cacheProvider;
            _accountRepositoryAsync = accountRepositoryAsync;
        }
        public async Task<Response<AuthenticationResponse>> Handle(GenerateUserLoginTokenQuery request, CancellationToken cancellationToken)
        {
            var getUserToken = await _accountRepositoryAsync.GenerateUserTokenQuery(request.UserName, request.Password);
            if (getUserToken.Data!= null)
            {
                Log.ForContext("UniqueId", Guid.NewGuid().ToString("N").Substring(0, 20).ToUpper())
                  .Information("GenerateTokenQuery: Received Token => {@Token}", getUserToken.Data?.UserDetails?.jwToken);
            }
            return new Response<AuthenticationResponse>(getUserToken.Data, message: "Token Received");
        }
    }
}
