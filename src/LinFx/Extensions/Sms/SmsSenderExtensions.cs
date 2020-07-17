using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace LinFx.Extensions.Sms
{
    [Obsolete]
    public static class SmsSenderExtensions
    {
        public static Task SendSmsAsync([NotNull] this ISmsSender smsSender, [NotNull] string phoneNumber, [NotNull] string text)
        {
            Check.NotNull(smsSender, nameof(smsSender));
            return smsSender.SendSmsAsync(new SmsMessage(phoneNumber, text));
        }
    }
}
