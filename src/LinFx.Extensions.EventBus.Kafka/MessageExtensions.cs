using Confluent.Kafka;
using System.Text;

namespace LinFx.Extensions.EventBus.Kafka;

public static class MessageExtensions
{
    public static string? GetMessageId<TKey, TValue>(this Message<TKey, TValue> message)
    {
        string? messageId = null;

        if (message.Headers.TryGetLastBytes("messageId", out var messageIdBytes))
            messageId = Encoding.UTF8.GetString(messageIdBytes);

        return messageId;
    }
}
