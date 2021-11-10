using LinFx.Extensions.EntityFrameworkCore.ObjectExtending;
using LinFx.Extensions.ObjectExtending;
using Microsoft.EntityFrameworkCore;

namespace LinFx.Extensions.EntityFrameworkCore.Modeling
{
    public static class ModelBuilderObjectExtensions
    {
        public static void TryConfigureObjectExtensions<TDbContext>(this ModelBuilder modelBuilder)
            where TDbContext : DbContext
        {
            ObjectExtensionManager.Instance.ConfigureEfCoreDbContext<TDbContext>(modelBuilder);
        }
    }
}
