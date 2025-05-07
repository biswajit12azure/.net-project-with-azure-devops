using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGL.Auth.Domain.Entities;

namespace WGL.Auth.Application.Interfaces.UserPortalRoleMapping
{
    public interface IUserPortalRoleMappingRepositoryAsync
    {
        Task<List<PortalRoleAccessResponse>> GetUserPortalRoleMappings(UserPortalRoleMappingParams userPortalRoleMapping);
        Task<int> BulkUpdateAccessMappings(IEnumerable<BulkUpdateRequest> items);
    }

}
