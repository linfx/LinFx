using System.Threading.Tasks;

namespace LinFx.Extensions.EntityFrameworkCore
{
    public interface IDbContextProvider<TDbContext> where TDbContext : IEfCoreDbContext
    {
        Task<TDbContext> GetDbContextAsync();
    }
}
