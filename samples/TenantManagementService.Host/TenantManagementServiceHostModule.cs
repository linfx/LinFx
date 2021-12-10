using LinFx.Extensions.AspNetCore.Uow;
using LinFx.Extensions.AuditLogging;
using LinFx.Extensions.AuditLogging.EntityFrameworkCore;
using LinFx.Extensions.Modularity;
using LinFx.Extensions.PermissionManagement;
using LinFx.Extensions.PermissionManagement.EntityFrameworkCore;
using LinFx.Extensions.TenantManagement;
using LinFx.Extensions.TenantManagement.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

namespace TenantManagementService.Host
{
    [DependsOn(
        //typeof(AuditLoggingModule),
        //typeof(PermissionManagementModule),
        typeof(TenantManagementModule)
    )]
    public class TenantManagementServiceHostModule : Module
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<TenantManagementDbContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseSqlite("Data Source=tenant.db", b => b.MigrationsAssembly("TenantManagementService.Host"));
            });

            services.AddDbContextPool<PermissionManagementDbContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseSqlite("Data Source=tenant.db", b => b.MigrationsAssembly("TenantManagementService.Host"));
            });

            services.AddDbContextPool<AuditLoggingDbContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseSqlite("Data Source=tenant.db", b => b.MigrationsAssembly("TenantManagementService.Host"));
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Tenant Management Service Api", Version = "v1" });
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "LinFx.Extensions.TenantManagement.xml"), true);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "LinFx.Extensions.TenantManagement.HttpApi.xml"), true);
                //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "LinFx.Extensions.PermissionManagement.xml"), true);
                //options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "LinFx.Extensions.PermissionManagement.HttpApi.xml"), true);
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
            });
        }

        public override void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Tenant Management Service Api");
            });

            app.UseUnitOfWork();
        }
    }
}
