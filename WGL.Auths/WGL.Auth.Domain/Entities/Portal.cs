using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGL.Auth.Domain.Common;

namespace WGL.Auth.Domain.Entities
{
    public class Portal
    {
        public int PortalID { get; set; }
        public string? PortalName { get; set; }
        public string? PortalDescription { get; set; }
        public bool IsActive { get; set; }
        public string? PortalKey { get; set; }

    }
}
