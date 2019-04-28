using System.Threading.Tasks;

namespace LinFx.Extensions.Sms
{
    public interface ISmsSender
    {
        Task SendSmsAsync(SmsMessage message);
    }
}
