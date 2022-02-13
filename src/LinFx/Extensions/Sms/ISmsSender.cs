using System.Threading.Tasks;

namespace LinFx.Extensions.Sms;

public interface ISmsSender
{
    Task SendAsync(SmsMessage smsMessage);
}
