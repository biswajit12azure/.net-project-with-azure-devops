using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGL.Auth.Domain.Common;

namespace WGL.Auth.Domain.Entities
{
    public class UserPortalRoleMappingParams : AuditableBaseEntity
    {
        //public int UserID { get; set; }
        public int PortalID { get; set; }
        public int RoleAccessMappingID { get; set; }
        //public int id { get; set; }
        public int AccessID { get; set; }
        public int RoleID { get; set; }
        public bool IsActive { get; set; }

    }

}
