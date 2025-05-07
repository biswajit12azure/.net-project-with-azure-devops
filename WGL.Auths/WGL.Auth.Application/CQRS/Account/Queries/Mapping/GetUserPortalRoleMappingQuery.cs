using AutoMapper;
using MediatR;
using WGL.Auth.Application.Interfaces.UserPortalRoleMapping;
using WGL.Utility.Wrappers;
using WGL.Auth.Domain.Entities;

namespace WGL.Auth.Application.CQRS.Account.Queries.Mapping
{
    public class GetUserPortalRoleMappingQuery : IRequest<Response<List<PortalRoleAccessResponse>>>
    {
        public required int PortalID { get; set; }
        public required int RoleAccessMappingID { get; set; }
        public required int AccessID { get; set; }
        public required int RoleID { get; set; }
        public required bool IsActive { get; set; }
    }
    public class GetUserPortalRoleMappingQueryHandler : IRequestHandler<GetUserPortalRoleMappingQuery, Response<List<PortalRoleAccessResponse>>>
    {
        private readonly IUserPortalRoleMappingRepositoryAsync _userPortalRole;
        private readonly IMapper _mapper;
        public GetUserPortalRoleMappingQueryHandler(IUserPortalRoleMappingRepositoryAsync userPortalRole, IMapper mapper)
        {
            _userPortalRole = userPortalRole;
            _mapper = mapper;
        }
        public async Task<Response<List<PortalRoleAccessResponse>>> Handle(GetUserPortalRoleMappingQuery request, CancellationToken cancellationToken)
        {
            var userPortalRole = _mapper.Map<UserPortalRoleMappingParams>(new UserPortalRoleMappingParams()
            {
                PortalID = request.PortalID,
                RoleAccessMappingID = request.RoleAccessMappingID,
                AccessID = request.AccessID,
                RoleID = request.RoleID,
                IsActive = request.IsActive,
                CreatedBy = 0
            });

            var result = await _userPortalRole.GetUserPortalRoleMappings(userPortalRole);
            return new Response<List<PortalRoleAccessResponse>>(result, message: $"UserPortalRoleMappingResult Recieved");
        }


    }

}
