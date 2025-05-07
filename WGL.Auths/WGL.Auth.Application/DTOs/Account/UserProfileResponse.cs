using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGL.Auth.Application.DTOs.Master;

namespace WGL.Auth.Application.DTOs.Account
{
    public class UserProfileResponse
    {
        public List<UserRequestParms> User { get; set; }
        public List<AgencyRequestParms> Agency { get; set; }
        public List<RoleRequestParms> Roles { get; set; }
        public List<MarketerRequestParms> Marketer { get; set; }
    }
}
