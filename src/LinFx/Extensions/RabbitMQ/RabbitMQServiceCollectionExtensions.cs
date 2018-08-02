using LinFx;
using LinFx.Extensions.RabbitMQ;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RabbitMQServiceCollectionExtensions
    {
        public static ILinFxBuilder AddRabbitMQ(this ILinFxBuilder builder, Action<RabbitMQOptions> optionsAction)
        {
            Check.NotNull(builder, nameof(builder));
            Check.NotNull(optionsAction, nameof(optionsAction));

            builder.Services.Configure(optionsAction);
            builder.Services.AddSingleton<RabbitMQContext>();

            return builder;
        }
    }
}