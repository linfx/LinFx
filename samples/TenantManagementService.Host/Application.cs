using LinFx.Extensions.Autofac;
using LinFx.Extensions.Modularity;
using LinFx.Extensions.TenantManagement.EntityFrameworkCore;
using LinFx.Extensions.TenantManagement.HttpApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace TenantManagementService;

[DependsOn(
    typeof(AutofacModule),
    typeof(TenantManagementHttpApiModule)
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

        app.UseExceptionHandling();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Tenant Management Service Api");
        });
        //app.UseAuditing();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapDefaultControllerRoute();
        });
    }
}