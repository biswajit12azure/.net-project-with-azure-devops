namespace WGL.Auth.Application.DTOs.Account
{
    public class AuthenticationResponse
    {
        public Userdetails? UserDetails { get; set; }
        public IEnumerable<Useraccess>? UserAccess { get; set; }
    }

    public class Userdetails
    {
        public int? id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? email { get; set; }
        public bool isVerified { get; set; }
        public string? jwToken { get; set; }
        public string? Status { get; set; }
        public DateTime tokenExpiry { get; set; }
    }

    public class Useraccess
    {
        public int PortalId { get; set; }
        public string? PortalName { get; set; }
        public string? PortalKey { get; set; }
        public bool IsMandateDone { get; set; }
        public int RoleId { get; set; }
        public string? Role { get; set; }
        public IEnumerable<Roleaccess>? RoleAccess { get; set; }
    }

    public class Roleaccess
    {
        public string? AccessName { get; set; }
        public string? AccessKey { get; set; }
        public bool IsActive { get; set; }
    }

}
