using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGL.Utility.EmailService
{
    public interface IEmailServiceAsync
    {
        Task SendAsync(EmailRequest request);
    }
}
