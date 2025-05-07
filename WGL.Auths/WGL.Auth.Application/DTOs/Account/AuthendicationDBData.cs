using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGL.Auth.Application.DTOs.Account
{
    public class AuthendicationDBData
    {
        public int UserID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailID { get; set; }
        public bool IsuserActive { get; set; }
        public bool IsMandateDone { get; set; }
        public int PortalId { get; set; }
        public string? PortalName { get; set; }
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public int AccessId { get; set; }
        public string? AccessName { get; set; }
        public bool IsAccessActive { get; set; }
        public string? Status { get; set; }
        public string? PortalKey { get; set; }
        public string? AccessKey { get; set; }
    }
}
