using LinFx.Extensions.AspNetCore.Mvc;
using LinFx.Extensions.Autofac;
using LinFx.Extensions.Modularity;
using LinFx.Extensions.PermissionManagement;
using Microsoft.OpenApi.Models;

namespace IdentityService;

[DependsOn(
    typeof(AutofacModule),
    typeof(AspNetCoreMvcModule),
    typeof(FeatureManagementModule)
    //typeof(AuditLoggingModule),
    //typeof(AccountHttpApiModule),
    //typeof(PermissionManagementHttpApiModule)
)]
public class Application : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity Service Api", Version = "v1" });
            //options.SwaggerDoc("v1", new OpenApiInfo { Title = "Tenant Management Service Api", Version = "v1" });
            options.DocInclusionPredicate((docName, description) => true);
            options.CustomSchemaIds(type => type.FullName);
        });

        services.AddControllers();

        //services.AddDbContext<TenantManagementDbContext>(options =>
        //{
        //    options.UseSqlite(options => options.MigrationsAssembly(GetType().Assembly.FullName));
        //});

        //services.AddDbContext<ApplicationDbContext>(options =>
        //{
        //    options.UseSqlite(options => options.MigrationsAssembly(GetType().Assembly.FullName));
        //});

        //services.AddDbContext<AuditLoggingDbContext>(options =>
        //{
        //    options.UseSqlite(options => options.MigrationsAssembly(GetType().Assembly.FullName));
        //});

        //services.Configure<DbContextOptions<AuditLoggingDbContext>>(options =>
        //{
        //    options.UseSqlite(options => options.MigrationsAssembly(GetType().Assembly.FullName));
        //});

        //services.AddDbContext<PermissionManagementDbContext>(options =>
        //{
        //    options.UseSqlite(options => options.MigrationsAssembly(GetType().Assembly.FullName));
        //});

        services
            .AddAuthentication()
            .AddJwtBearer();

        //services
        //    .AddIdentity<IdentityUser, IdentityRole>()
        //    .AddEntityFrameworkStores<ApplicationDbContext>()
        //    .AddDefaultTokenProviders();
    }
}