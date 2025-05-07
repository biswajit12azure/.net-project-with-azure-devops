using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGL.Auth.Application.DTOs.Account;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Utility.Wrappers;

namespace WGL.Auth.Application.CQRS.Account.Queries.GetUserProfileByPortalID
{
    public class GetUserProfileByPortalIDHandler:IRequest<Response<List<UserProfileResponse>>>
    {
        public int PortalID { get; set; }
        
    }
    public class GetUserProfileByPortalIDHandlerQueryHandler : IRequestHandler<GetUserProfileByPortalIDHandler, Response<List<UserProfileResponse>>>
    {
        public IAccountRepositoryAsync _accountRepositoryAsync;
        public GetUserProfileByPortalIDHandlerQueryHandler(IAccountRepositoryAsync accountRepositoryAsync)
        {
            _accountRepositoryAsync = accountRepositoryAsync;
        }
        public async Task<Response<List<UserProfileResponse>>> Handle(GetUserProfileByPortalIDHandler request, CancellationToken cancellationToken)
        {
            var response = await _accountRepositoryAsync.GetUserProfileByPortalID(request.PortalID);
            return new Response<List<UserProfileResponse>>(response);


        }
    }
}
