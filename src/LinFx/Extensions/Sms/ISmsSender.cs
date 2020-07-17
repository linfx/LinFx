using System;
using System.Threading.Tasks;

namespace LinFx.Extensions.Sms
{
    [Obsolete]
    public interface ISmsSender
    {
        Task SendSmsAsync(SmsMessage message);
    }
}
