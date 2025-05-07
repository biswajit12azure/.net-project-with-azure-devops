using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGL.Auth.Application.DTOs.Master;

namespace WGL.Auth.Application.DTOs.Account
{
    public class UserResponseSDDTO
    {
        public int? UserID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CompanyName { get; set; }
        public string? MobileNumber { get; set; }
        public string? EmailID { get; set; }
        public bool? IsActive { get; set; }
        public int? StatusID { get; set; }
        public int? AdditionalID { get; set; }
        //public int UserID { get; set; }
       // public string? CompanyName { get; set; }
        public string? ContactPerson { get; set; }
        public string? Title { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? CompanyWebsite { get; set; }
        public string? Email { get; set; }
        public string? ZipCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Fax { get; set; }
        public string? CellPhone { get; set; }
        public int CategoryID { get; set; }
        public int ClassificationID { get; set; }
        public string? ServicesProductsProvided { get; set; }
        //public UserDTO user { get; set; }
        //public AdditionalSDDTO additionalMC { get; set; }
        // public HomeAddressMCDto homeAddressMCDto { get; set; }
        public List<DocumentDTO> documents { get; set; }

    }
}
