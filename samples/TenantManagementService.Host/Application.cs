using LinFx.Extensions.Autofac;
using LinFx.Extensions.Modularity;
using LinFx.Extensions.TenantManagement;
using LinFx.Extensions.TenantManagement.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace TenantManagementService;

[DependsOn(
    typeof(AutofacModule),
    typeof(TenantManagementModule)
)]
public class Application : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Tenant Management Service Api", Version = "v1" });
            options.DocInclusionPredicate((docName, description) => true);
            options.CustomSchemaIds(type => type.FullName);
        });

        services.AddDbContext<TenantManagementDbContext>(options =>
        {
            options.UseSqlite(options => options.MigrationsAssembly(GetType().Assembly.FullName));
        });
    }
}