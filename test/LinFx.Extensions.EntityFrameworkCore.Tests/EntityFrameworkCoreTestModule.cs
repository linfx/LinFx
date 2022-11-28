using LinFx.Extensions.Modularity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Extensions.EntityFrameworkCore.Tests
{
    class EntityFrameworkCoreTestModule : Module
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyDbContext>(options =>
            {
                /* Remove "includeAllEntities: true" to create
                * default repositories only for aggregate roots */
                //options.AddDefaultRepositories(includeAllEntities: true);
                options.UseNpgsql("server=10.0.1.222;port=15432;database=db1;username=postgres;password=123456;");
            });
        }
    }
}
