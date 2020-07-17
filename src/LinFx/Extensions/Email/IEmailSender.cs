using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.Email
{
    [Obsolete]
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, bool isHtml = false);
    }
}
