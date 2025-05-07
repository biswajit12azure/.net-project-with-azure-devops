using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGL.Utility.EmailService
{
    public class EmailRequest
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string From { get; set; }
        public string EmailPassword { get; set; }
        public string UserId { get; set; }
    }


    public class MailSettings
    {
        public string AppEmailFrom { get; set; }
        public string AppEmailPwd { get; set; }
        public string AppEmailUser { get; set; }
        public string ExchangeHost { get; set; }
        public string EmailAuthUser { get; set; }
    }

    public class OAuthWGL
    {
        public string ApplicationId { get; set; }
        public string TenantId { get; set; }
        public string ClientSecret { get; set; }
        public string Scopes { get; set; }

    }

}
