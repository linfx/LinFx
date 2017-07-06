using LinFx.Data;
using LinFx.Domain.Services;
using LinFx.Domain.Uow;
using LinFx.SaaS.Domain.Entities;
using System.Threading.Tasks;

namespace LinFx.SaaS.Organizations
{
	public class OrganizationUnitManager : DomainService
    {
		protected IRepository<OrganizationUnit> OrganizationUnitRepository { get; private set; }

		public virtual async Task CreateAsync(OrganizationUnit organizationUnit)
		{
			await OrganizationUnitRepository.InsertAsync(organizationUnit);
		}

		public virtual async Task UpdateAsync(OrganizationUnit organizationUnit)
		{
			await OrganizationUnitRepository.UpdateAsync(organizationUnit);
		}

		[UnitOfWork]
		public virtual async Task DeleteAsync(string id)
		{
			//var children = await FindChildrenAsync(id);
			//foreach(var child in children)
			//{
			//	await OrganizationUnitRepository.DeleteAsync(child);
			//}
			await OrganizationUnitRepository.DeleteAsync(id);
		}

		//public async Task<OrganizationUnit[]> FindChildrenAsync(string parentId)
		//{
		//	return null;
		//}
	}
}
