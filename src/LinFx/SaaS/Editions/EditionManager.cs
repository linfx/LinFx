//using LinFx.Data;
//using LinFx.Domain.Services;
//using LinFx.Extensions;
//using LinFx.SaaS.Features;
//using System.Threading.Tasks;

//namespace LinFx.SaaS.Editions
//{
//    public class EditionManager : DomainService<Edition>
//    {
//        private IFeatureValueStore _featureValueStore;

//        protected IFeatureManager FeatureManager { get; set; }

//        public EditionManager(IRepository<Edition> repository) : base(repository) { }

//        public virtual Task<string> GetFeatureValueOrNullAsync(int editionId, string featureName)
//        {
//            return _featureValueStore.GetEditionValueOrNullAsync(editionId, featureName);
//        }

//        public virtual async Task SetFeatureValuesAsync(int editionId, params NameValue[] values)
//        {
//            if (values.IsNullOrEmpty())
//            {
//                return;
//            }

//            foreach (var value in values)
//            {
//                await SetFeatureValueAsync(editionId, value.Name, value.Value);
//            }
//        }

//        public virtual Task SetFeatureValueAsync(int editionId, string featureName, string value)
//        {
//            return _featureValueStore.SetEditionFeatureValueAsync(editionId, featureName, value);
//        }
//    }
//}