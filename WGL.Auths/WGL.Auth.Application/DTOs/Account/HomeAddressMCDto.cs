using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGL.Auth.Application.DTOs.Account
{
    public class HomeAddressMCDto
    {
        public int? ID { get; set; }
        public int? AdditionalID { get; set; }
        public string? StreetAddress1 { get; set; }
        public string? StreetAddress2 { get; set; }
        public string? City { get; set; }
        public string? FirstName { get; set; }
        public string? State { get; set; }
        public string? ZIPCODE { get; set; }
    }
}
