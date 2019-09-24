using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LinFx.Extensions.Email
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly ILogger<SmtpEmailSender> _logger;
        private readonly SmtpEmailOptions _options;
        private readonly SmtpClient _client;

        public SmtpEmailSender(
            ILogger<SmtpEmailSender> logger,
            IOptions<SmtpEmailOptions> options)
        {
            _logger = logger;
            _options = options.Value;
            _client = new SmtpClient
            {
                Host = _options.Host,
                Port = _options.Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = _options.UseSSL,
                Credentials = new System.Net.NetworkCredential(_options.Login, _options.Password)
            };
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            _logger.LogInformation($"Sending email: {email}, subject: {subject}, message: {htmlMessage}");
            try
            {
                var mail = new MailMessage(_options.Login, email)
                {
                    IsBodyHtml = true,
                    Subject = subject,
                    Body = htmlMessage
                };
                _client.Send(mail);
                _logger.LogInformation($"Email: {email}, subject: {subject}, message: {htmlMessage} successfully sent");
                return Task.CompletedTask;
            }
            catch (SmtpException ex)
            {
                _logger.LogError($"Exception {ex} during sending email: {email}, subject: {subject}");
                throw;
            }
        }
    }
}