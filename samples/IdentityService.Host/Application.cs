﻿using IdentityService.EntityFrameworkCore;
using LinFx.Extensions.Account.HttpApi;
using LinFx.Extensions.AspNetCore.Mvc;
using LinFx.Extensions.AuditLogging;
using LinFx.Extensions.Autofac;
using LinFx.Extensions.Modularity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace IdentityService;

[DependsOn(
    typeof(AutofacModule),
    typeof(AspNetCoreMvcModule),
    typeof(AuditLoggingModule),
    typeof(AccountHttpApiModule)
    //typeof(PermissionManagementHttpApiModule)
)]
public class Application : Module
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity Service Api", Version = "v1" });
            options.DocInclusionPredicate((docName, description) => true);
            options.CustomSchemaIds(type => type.FullName);
        });

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite(options => options.MigrationsAssembly(GetType().Assembly.FullName));
        });

        //services.AddDbContext<PermissionManagementDbContext>(options =>
        //{
        //    options.UseSqlite(options => options.MigrationsAssembly(GetType().Assembly.FullName));
        //});

        services
            .AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
    }
}