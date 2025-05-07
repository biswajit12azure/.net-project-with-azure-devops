namespace WGL.Account.Domain.Settings
{
    public class AppSettings
    {

    }
    public class JWTSettings
    {
        public required string Key { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
    }
}
