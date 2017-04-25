using LinFx.SaaS.Features;
using System.Collections.Generic;

namespace LinFx.SaaS.Editions
{
    public class EditionFeatureSetting : FeatureSetting
    {
        public string EditionId { get; set; }
        /// <summary>
        /// Gets or sets the edition.
        /// </summary>
        public virtual Edition Edition { get; set; }
    }

    public class EditionFeatureCacheItem
    {
        public const string CacheStoreName = "EditionFeatures";

        public IDictionary<string, string> FeatureValues { get; set; }

        public EditionFeatureCacheItem()
        {
            FeatureValues = new Dictionary<string, string>();
        }
    }
}