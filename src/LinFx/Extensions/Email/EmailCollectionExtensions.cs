using LinFx.Extensions.Email;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EmailCollectionExtensions
    {
        public static LinFxBuilder AddEmail(this LinFxBuilder fx, Action<EmailSenderOptions> optionsAction)
        {
            fx.Services.Configure(optionsAction);
            fx.Services.AddSingleton<IEmailSender, SmtpEmailSender>();
            return fx;
        }
    }
}