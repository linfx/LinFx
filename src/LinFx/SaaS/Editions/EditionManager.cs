using LinFx.Data;
using LinFx.Domain.Repositories;
using LinFx.Domain.Services;
using LinFx.SaaS.Features;

namespace LinFx.SaaS.Editions
{
    public class EditionManager : DomainService<Edition>
    {
        protected IFeatureManager FeatureManager { get; set; }

        public EditionManager(IRepository<Edition> repository) : base(repository) { }
    }
}