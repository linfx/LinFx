namespace LinFx.Extensions.Elasticsearch
{
    public class ElasticsearchOptions
    {
        /// <summary>
        /// Host (default: http://localhost:9200)
        /// </summary>
        public string Host { get; set; } = "http://localhost:9200";

        /// <summary>
        /// Default Index
        /// </summary>
        public string Index { get; set; }
    }
}