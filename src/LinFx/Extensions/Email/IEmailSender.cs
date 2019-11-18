using System.Threading.Tasks;

namespace LinFx.Extensions.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, bool isHtml = false);
    }
}
