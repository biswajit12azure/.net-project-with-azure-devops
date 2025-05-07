using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGL.Auth.Domain.Entities
{
    public class BulkUpdateRequest
    {
        public int RoleAccessMappingID { get; set; }
        public bool IsActive { get; set; }
    }
}
