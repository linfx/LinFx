using LinFx;
using LinFx.Extensions.Mongo;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MongoServiceCollectionExtensions
    {
        public static ILinFxBuilder AddMongoDBContext(this ILinFxBuilder builder, Action<MongoDbOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));
            Check.NotNull(optionsAction, nameof(optionsAction));

            builder.Services.Configure(optionsAction);
            builder.Services.AddSingleton<MongoDbContext>();

            return builder;
        }
    }
}
