using DoorlockServerApplication.Extensions;
using IdentityService.EntityFrameworkCore;
using LinFx.Extensions.AspNetCore.ExceptionHandling;
using LinFx.Extensions.AspNetCore.Mvc;
using LinFx.Extensions.Autofac;
using LinFx.Extensions.ExceptionHandling;
using LinFx.Extensions.FeatureManagement;
using LinFx.Extensions.Modularity;
using LinFx.Extensions.PermissionManagement;
using LinFx.Extensions.TenantManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace IdentityService;

[DependsOn(
    typeof(AutofacModule),
    typeof(AspNetCoreMvcModule),
    typeof(FeatureManagementModule),
    //typeof(AuditLoggingModule),
    //typeof(AccountHttpApiModule),
    typeof(PermissionManagementModule)
)]
public class Application : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        var configuration = services.GetConfiguration();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity Service Api", Version = "v1" });
            options.DocInclusionPredicate((docName, description) => true);
            options.CustomSchemaIds(type => type.FullName);
        });

        services
            .AddAuthentication()
            .AddJwtBearer();

        //services.Configure<DbContextOptions<AuditLoggingDbContext>>(options =>
        //{
        //    options.UseSqlite(options => options.MigrationsAssembly(GetType().Assembly.FullName));
        //});

        services
            .AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(options => options.MigrationsAssembly(GetType().Assembly.FullName));
            })
            .AddDbContext<TenantManagementDbContext>(options =>
            {
                options.UseSqlite(options => options.MigrationsAssembly(GetType().Assembly.FullName));
            })
            //.AddDbContext<AuditLoggingDbContext>(options =>
            //{
            //    options.UseSqlite(options => options.MigrationsAssembly(GetType().Assembly.FullName));
            //})
            .AddDbContext<PermissionManagementDbContext>(options =>
            {
                options.UseSqlite(options => options.MigrationsAssembly(GetType().Assembly.FullName));
            })
            .AddDbContext<FeatureManagementDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("Default"));
            });

        services
            .AddTransient<IHttpExceptionStatusCodeFinder, HttpExceptionStatusCodeFinder>()
            .AddTransient<IExceptionToErrorInfoConverter, ExceptionToErrorInfoConverter>();


        //services
        //    .AddIdentity<IdentityUser, IdentityRole>()
        //    .AddEntityFrameworkStores<ApplicationDbContext>()
        //    .AddDefaultTokenProviders();
    }
}