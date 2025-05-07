﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGL.Auth.Domain.Common;

namespace WGL.Auth.Domain.Entities
{
    public class ApplicationUser : AuditableBaseEntity
    {
        public int? UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public required string FullName { get; set; }
        public required string CompanyName { get; set; }
        public required string MobileNumber { get; set; }
        public required string EmailID { get; set; }
        public required string Password { get; set; }
        public required int PortalId { get; set; }

    }
}
