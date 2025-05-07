using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGL.Auth.Domain.Entities
{
    public class AdditionalSD
    {
        public int? AdditionalID { get; set; }
        public int UserID { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactPerson { get; set; }
        public string? Title { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public int? State { get; set; }
        public string? CompanyWebsite { get; set; }
        public string? Email { get; set; }
        public string? ZipCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Fax { get; set; }
        public string? CellPhone { get; set; }
        public int CategoryID { get; set; }
        public string ClassificationID { get; set; }
        public string? ServicesProductsProvided { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int AgencyID { get; set; }
        public int AgencyStateID { get; set; }
        public List<Document> FileData { get; set; }
    }

    public class SupplierDiversityUserById {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string MobileNumber { get; set; }
        public int StatusID { get; set; }
        public bool IsActive { get; set; }
        public AdditionalSD additionalSD { get; set; }
    }
}
