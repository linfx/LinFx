using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace LinFx.Extensions.Sms
{
    public class SmsMessage
    {
        public string PhoneNumber { get; }

        public string Text { get; }

        public IDictionary<string, object> Properties { get; }

        public SmsMessage([NotNull] string phoneNumber, [NotNull] string text)
        {
            PhoneNumber = phoneNumber;
            Text = text;
            Properties = new Dictionary<string, object>();
        }
    }
}
