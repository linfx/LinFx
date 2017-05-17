using LinFx.Data;
using LinFx.SaaS.Editions;
using LinFx.SaaS.MultiTenancy;
using System;
using System.Threading.Tasks;

namespace LinFx.SaaS.Features
{
    /// <summary>
    /// Defines a store to get feature values.
    /// </summary>
    public interface IFeatureValueStore
    {
        /// <summary>
        /// Gets the feature value or null.
        /// </summary>
        /// <param name="tenantId">The tenant id.</param>
        /// <param name="feature">The feature.</param>
        //Task<string> GetValueOrNullAsync(int tenantId, Feature feature);
        Task<string> GetValueOrNullAsync(int tenantId, string featureName);
        Task<string> GetEditionValueOrNullAsync(int editionId, string featureName);
        Task SetEditionFeatureValueAsync(int editionId, string featureName, string value);
    }

    public class FeatureValueStore : IFeatureValueStore
    {
        private readonly IRepository<TenantFeatureSetting> _tenantFeatureRepository;
        private readonly IRepository<EditionFeatureSetting> _editionFeatureRepository;

        public Task<string> GetEditionValueOrNullAsync(int editionId, string featureName)
        {
            throw new NotImplementedException();
        }

        //public Task<string> GetValueOrNullAsync(int tenantId, Feature feature)
        //{
        //    throw new NotImplementedException();
        //}

        public Task<string> GetValueOrNullAsync(int tenantId, string featureName)
        {
            throw new NotImplementedException();
        }

        public Task SetEditionFeatureValueAsync(int editionId, string featureName, string value)
        {
            throw new NotImplementedException();
        }
    }
}
