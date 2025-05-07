using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGL.Auth.Application.DTOs.Master;

namespace WGL.Auth.Application.DTOs.Account
{
    public class AdditionalMCDTO
    {
        public int? ID { get; set; }
        public int? UserID { get; set; }
        public string? FullName { get; set; }
        public string? AlternateEmail { get; set; }
        public string? DLState { get; set; }
        public string? DLNumber { get; set; }
        //public string? HomeStreetAddress1 { get; set; }
        //public string? HomeStreetAddress2 { get; set; }
        //public string? HomeCity { get; set; }
        //public string? HomeState { get; set; }
        //public string? HomeZipCode { get; set; }
        public string? CompanyName { get; set; }
        public string? TaxIdentificationNumber { get; set; }
        public string? CompanyStreetAddress1 { get; set; }
        public string? CompanyStreetAddress2 { get; set; }
        public string? CompanyCity { get; set; }
        public string? CompanyState { get; set; }
        public string? CompanyZipCode { get; set; }
        public string? CompanyContactName { get; set; }
        public string? CompanyContactTelephone { get; set; }
        public string? CompanyContactEmailAddress { get; set; }
        public string? AuthorizedWGLContact { get; set; }
        //public HomeAddressMCDto homeAddressMcDto { get; set; }
        public List<DocumentDTO>? FileData { get; set; }
    }
}
