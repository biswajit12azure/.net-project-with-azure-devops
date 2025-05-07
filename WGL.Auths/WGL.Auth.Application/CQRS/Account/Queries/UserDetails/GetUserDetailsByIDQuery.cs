using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGL.Auth.Application.CQRS.Account.Queries.DocumentType;
using WGL.Auth.Application.DTOs.Account;
using WGL.Auth.Application.Interfaces.Account;
using WGL.Auth.Domain.Entities;
using WGL.Utility.Wrappers;

namespace WGL.Auth.Application.CQRS.Account.Queries.UserDetails
{
    
    public class GetUserDetailsByIDQuery: IRequest<Response<UserDetailsResponse>>
    {
        public int UserID { get; set; }
        public int PortalID { get; set; }
        public GetUserDetailsByIDQuery(int userID, int portalID)
        {
            UserID = userID;
            portalID = PortalID;
        }

    }
    public class GetUserDetailsByIDQueryHandler : IRequestHandler<GetUserDetailsByIDQuery, Response<UserDetailsResponse>>
    {
        private readonly IAccountRepositoryAsync _userDetails;
        private readonly IMapper _mapper;
        public GetUserDetailsByIDQueryHandler(IAccountRepositoryAsync userDetails, IMapper mapper)
        {
            _userDetails = userDetails;
            _mapper = mapper;
        }
        public async Task<Response<UserDetailsResponse>> Handle(GetUserDetailsByIDQuery request, CancellationToken cancellationToken)
        {
            var result = await _userDetails.GetUserDetailsByID(request.UserID,request.PortalID);
            return new Response<UserDetailsResponse>(result, message: "User Details Received Successfully!!");
        }
    }
}
