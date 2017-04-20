using LinFx.Data;
using LinFx.Domain.Entities;
using LinFx.SaaS.Features;
using System.Threading.Tasks;

namespace LinFx.SaaS.Editions
{
    public class EditionManager
    {
        protected IRepository<Edition> _editionRepository { get; set; }

        protected IFeatureManager FeatureManager { get; set; }

        public EditionManager(IRepository<Edition> editionRepository)
        {
            _editionRepository = editionRepository;
        }

        public virtual Task CreateAsync(Edition item)
        {
            item.NewId();
            return _editionRepository.InsertAsync(item);
        }

        public virtual Task UpdateAsync(Edition item)
        {
            return _editionRepository.UpdateAsync(item);
        }

        public virtual Task DeleteAsync(Edition item)
        {
            return _editionRepository.DeleteAsync(item);
        }

        public virtual Task<Edition> GetByIdAsync(string id)
        {
            return _editionRepository.GetAsync(id);
        }

        //public virtual Task<Edition> FindByIdAsync(string id)
        //{
        //    return EditionRepository.FirstOrDefaultAsync(id);
        //}

        //public virtual Task<Edition> FindByNameAsync(string name)
        //{
        //    return EditionRepository.FirstOrDefaultAsync(edition => edition.Name == name);
        //}
    }
}