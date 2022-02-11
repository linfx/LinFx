using LinFx.Extensions.AuditLogging;
using LinFx.Extensions.AuditLogging.EntityFrameworkCore;
using LinFx.Extensions.Autofac;
using LinFx.Extensions.EntityFrameworkCore;
using LinFx.Extensions.Modularity;
using LinFx.Extensions.PermissionManagement;
using LinFx.Extensions.PermissionManagement.EntityFrameworkCore;
using LinFx.Extensions.TenantManagement.EntityFrameworkCore;
using LinFx.Extensions.TenantManagement.HttpApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TenantManagementService.Extensions;

namespace TenantManagementService;

[DependsOn(
    typeof(AutofacModule),
    typeof(AuditLoggingModule),
    typeof(PermissionManagementModule),
    typeof(TenantManagementHttpApiModule)
)]
public class TenantManagementHostModule : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        Configure<EfCoreDbContextOptions>(options =>
        {
            options.UseSqlite<AuditLoggingDbContext>();
            options.UseSqlite<PermissionManagementDbContext>();
            options.UseSqlite<TenantManagementDbContext>();
        });

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Tenant Management Service Api", Version = "v1" });
            //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "LinFx.Extensions.TenantManagement.xml"), true);
            //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "LinFx.Extensions.TenantManagement.HttpApi.xml"), true);
            //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "LinFx.Extensions.PermissionManagement.xml"), true);
            //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "LinFx.Extensions.PermissionManagement.HttpApi.xml"), true);
            options.DocInclusionPredicate((docName, description) => true);
            options.CustomSchemaIds(type => type.FullName);
        });
    }

    public override void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Tenant Management Service Api");
        });
        app.UseAuditing();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });
    }
}
