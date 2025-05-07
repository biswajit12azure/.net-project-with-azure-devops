using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGL.Auth.Domain.Entities
{
    public class UserResponseMC_toDelete
    {
        public MapCentreUserById UserDetails { get; set; }
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string DLState { get; set; }
        public string DLNumber { get; set; }
        public string HomeStreetAddress1 { get; set; }
        public string HomeStreetAddress2 { get; set; }
        public string HomeCity { get; set; }
        public string HomeState { get; set; }
        public string HomeZipCode { get; set; }
        public string CompanyName { get; set; }
        public string TaxIdentificationNumber { get; set; }
        public string CompanyStreetAddress1 { get; set; }
        public string CompanyStreetAddress2 { get; set; }
        public string CompanyCity { get; set; }
        public string CompanyState { get; set; }
        public string CompanyZipCode { get; set; }
        public string CompanyContactName { get; set; }
        public string CompanyContactTelephone { get; set; }
        public string CompanyContactEmailAddress { get; set; }
        public string AuthorizedWGLContact { get; set; }
        public int AdditionalID { get; set; }
        public List<Document> FileData { get; set; }
        public List<DocumentData> DocumentData { get; set; }
        public List<StateData> State { get; set; }
    }
    public class MapCentreUserById
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string MobileNumber { get; set; }
        public int StatusID { get; set; }
        public bool IsActive { get; set; }
        public List<AdditionalMC> additionalMC { get; set; }
    }

    // new domain classes by praveen


    public class DocumentData
    {
        public string DocumentTypeID { get; set; }
        public string DocumentType { get; set; }
        public string DocumentDescription { get; set; }
    }

    public class StateData
    {
        public string StateId { get; set; }
        public string StateName { get; set; }
    }

   

    public class ApiResponse
    {
        public UserResponseMC_toDelete Data { get; set; }
    }
}
