using LinFx;
using LinFx.Extensions.MongoDB;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class MongoServiceCollectionExtensions
    {
        public static LinFxBuilder AddMongoDBContext(this LinFxBuilder builder, Action<MongoDbOptions> optionsAction)
        {
            Check.NotNull(builder, nameof(builder));
            Check.NotNull(optionsAction, nameof(optionsAction));

            builder.Services.Configure(optionsAction);
            builder.Services.AddSingleton<MongoDbContext>();

            return builder;
        }
    }
}
