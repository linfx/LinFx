using LinFx.Extensions.Elasticsearch;
using Nest;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ElasticsearchServiceCollectionExtensions
    {
        public static LinFxBuilder AddElasticsearch(this LinFxBuilder builder, Action<ElasticsearchOptions> optionsAction)
        {
            var options = new ElasticsearchOptions();
            optionsAction?.Invoke(options);

            var node = new Uri(options.Host);
            var settings = new ConnectionSettings(node).DefaultIndex(options.Index);
            var client = new ElasticClient(settings);
            builder.Services.Add(ServiceDescriptor.Singleton<IElasticClient>(client));

            return builder;
        }
    }
}