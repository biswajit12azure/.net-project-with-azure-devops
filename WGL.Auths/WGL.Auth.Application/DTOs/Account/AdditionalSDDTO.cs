using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WGL.Auth.Application.DTOs.Master;

namespace WGL.Auth.Application.DTOs.Account
{
    public class AdditionalSDDTO
    {
        public int? AdditionalID { get; set; }
        public int UserID { get; set; }
        public string? CompanyName { get; set; }
        public string? ContactPerson { get; set; }
        public string? Title { get; set; }
        public string? Address { get; set; }
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
        public List<DocumentDTO> FileData { get; set; }
        public List<DocumentTypeDTO> DocumentData { get; set; }
        public List<StateDTO> State1 { get; set; }
        public List<BusinessCategoryDTO> BusinessCategory { get; set; }
        public List<ClassificationDTO> Classification { get; set; }
        public List<AgencyDTO> Agency { get; set; }

    }
}
