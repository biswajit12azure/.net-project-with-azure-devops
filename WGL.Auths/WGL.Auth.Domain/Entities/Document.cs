using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGL.Auth.Domain.Entities
{
    public class Document
    {
        public int? ID { get; set; }
        public int AdditionalID { get; set; }
        public int? DocumentTypeID { get; set; }
        public string? FileName { get; set; }
        public string? Format { get; set; }
        public int? Size { get; set; }
        public string? PortalKey { get; set; }
        public string? File { get; set; }
        public string? Url { get; set; } 
    }
}
