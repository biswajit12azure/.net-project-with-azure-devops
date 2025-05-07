using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGL.Auth.Application.DTOs.Master
{
    public class DocumentDTO
    {
        public int? ID { get; set; }
        public int? AdditionalID { get; set; }
        public int? DocumentTypeID { get; set; }
        public string? FileName { get; set; }
        public string? Format { get; set; }
        public int? Size { get; set; }
        public string? PortalKey { get; set; }
        public string? File { get; set; }
        public string? Url { get; set; }
    }
    public class DocumentType
    {
        public int? DocumentID { get; set; }
        public string? DocumentName { get; set; }
        public string? DocumentDescription { get; set; }
    }

}
