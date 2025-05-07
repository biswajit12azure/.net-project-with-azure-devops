using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGL.Auth.Domain.Entities
{
    public class AdditionalMC
    {
        public int? AdditionalID { get; set; }
        public int UserID { get; set; }
        public string? FullName { get; set; }
        public string? AlternateEmail { get; set; }
        public int? DLState { get; set; }
        public string? DLNumber { get; set; }
        public string? HomeStreetAddress1 { get; set; }
        public string? HomeStreetAddress2 { get; set; }
        public string? HomeCity { get; set; }
        public int? HomeState { get; set; }
        public string? HomeZipCode { get; set; }
        public string? CompanyName { get; set; }
        public string? TaxIdentificationNumber { get; set; }
        public string? CompanyStreetAddress1 { get; set; }
        public string? CompanyStreetAddress2 { get; set; }
        public string? CompanyCity { get; set; }
        public int? CompanyState { get; set; }
        public string? CompanyZipCode { get; set; }
        public string? CompanyContactName { get; set; }
        public string? CompanyContactTelephone { get; set; }
        public string? CompanyContactEmailAddress { get; set; }
        public string? AuthorizedWGLContact { get; set; }
        public List<Document> FileData { get; set; }
    }

   
}
