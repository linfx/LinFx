using LinFx.Extensions.TenantManagement.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

namespace TenantManagementService.Host
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddLinFx()
                .AddHttpContextPrincipalAccessor()
                .AddThreading()
                .AddDataFilter()
                .AddDbContextProvider()
                .AddDbContext<TenantManagementDbContext>(option =>
                {
                    option.AddDefaultRepositories();
                })
                .AddTenantManagement();

            services.AddDbContextPool<TenantManagementDbContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseSqlite("Data Source=tenant.db", b => b.MigrationsAssembly("TenantManagementService.Host"));
            });

            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Tenant Management Service Api", Version = "v1" });
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "LinFx.Extensions.TenantManagement.xml"), true);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "LinFx.Extensions.TenantManagement.HttpApi.xml"), true);
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Tenant Management Service Api");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}