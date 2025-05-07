using Microsoft.Identity.Client;
using System.Net;
using MimeKit;
using WGL.Utility.Password;
using Microsoft.Exchange.WebServices.Data;

namespace WGL.Utility.EmailService
{
    public class EmailService : IEmailServiceAsync
    {

        private string applicationId;
       // private readonly IConfigurationManager configurationManager;
        private string tenantId;
        private string clientSecret;
        private string senderEmail;
        public MailSettings _mailSettings { get; }
        public OAuthWGL _oAuthWGL { get; }

        //public EmailService(IConfigurationManager configurationManager)
        //{
        //    this.configurationManager = configurationManager;
        //}
        public async System.Threading.Tasks.Task SendAsync(EmailRequest request)
        {
            try
            {
                Microsoft.Extensions.Configuration.ConfigurationManager config = new Microsoft.Extensions.Configuration.ConfigurationManager();
                // Triggering Email using ExchangeService for RMAs
                ExchangeService service = new(ExchangeVersion.Exchange2013) { Url = new Uri("https://outlook.office365.com/ews/exchange.asmx") };
                //service.Url = new Uri(_mailSettings.ExchangeHost);
                // service.Url = new Uri("https://outlook.office365.com/ews/exchange.asmx");
                EmailMessage email = new(service);
                applicationId = "f16517c2-5e4b-444f-9dda-61e8aa121d6b";
                clientSecret = "mFd8Q~aBP8Yg_Ur7qTmd0320RlPkGpRZ4OcoecGv";
                tenantId = "9b47157b-2ccc-421f-99e7-3b35746ab942";
                senderEmail = "no-reply.microncc@sutherlandglobal.com";

                var cca = ConfidentialClientApplicationBuilder
                             .Create(applicationId)
                             .WithClientSecret(clientSecret)
                             .WithTenantId(tenantId)
                             .Build();
                // var ewsScopes = new string[] { _oAuthWGL.Scopes };
                var ewsScopes = new string[] { "https://outlook.office365.com/.default" };
                AuthenticationResult authResult = null;
                authResult = cca.AcquireTokenForClient(ewsScopes).ExecuteAsync().Result;
                service.Credentials = new OAuthCredentials(authResult.AccessToken);
                service.ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.SmtpAddress, senderEmail);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                //Basic Authentication disabled by Microsoft.
                email.From = new EmailAddress(senderEmail);

                service.TraceEnabled = true;
                email.ToRecipients.Add(request.To);
                email.Subject = request.Subject;
                var builder = new BodyBuilder { HtmlBody = request.Body };
                //var builder = new BodyBuilder { HtmlBody = "<a href=\"http://localhost:3000/registration/verified?verifyId="+ PasswordEncoder.EncodePasswordToBase64(request.UserId) +"\">Verify Email</a>" };
                email.Body = builder.HtmlBody;

                email.SendAndSaveCopy();
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex.Message, ex);
                //throw new ApiException(ex.Message);
            }
        }
    }
}
