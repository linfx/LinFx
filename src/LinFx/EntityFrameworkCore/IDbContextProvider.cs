using System.Threading.Tasks;

namespace LinFx.EntityFrameworkCore
{
    public interface IDbContextProvider<TDbContext> where TDbContext : IEfCoreDbContext
    {
        Task<TDbContext> GetDbContextAsync();
    }
}
