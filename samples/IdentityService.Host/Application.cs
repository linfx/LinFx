using IdentityService.Controllers;
using IdentityService.EntityFrameworkCore;
using LinFx.Extensions.AspNetCore.ExceptionHandling;
using LinFx.Extensions.AspNetCore.Mvc;
using LinFx.Extensions.AuditLogging;
using LinFx.Extensions.AuditLogging.EntityFrameworkCore;
using LinFx.Extensions.Authorization;
using LinFx.Extensions.Autofac;
using LinFx.Extensions.ExceptionHandling;
using LinFx.Extensions.FeatureManagement;
using LinFx.Extensions.Modularity;
using LinFx.Extensions.PermissionManagement;
using LinFx.Extensions.TenantManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;

namespace IdentityService;

[DependsOn(
    typeof(AutofacModule),
    typeof(AspNetCoreMvcModule),
    typeof(AuthorizationModule),
    typeof(AuditLoggingModule),
    typeof(TenantManagementModule),
    typeof(FeatureManagementModule),
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

        RSA rsa = RSA.Create();
        rsa.ImportFromPem(AccountController.PUBLIC_KEY);

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new RsaSecurityKey(rsa);
        //var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("5172510c6f5640a796070c3cdf8a937e"));
        var creds = new SigningCredentials(key, SecurityAlgorithms.RsaSha256);

        services
            .AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    //ValidIssuer = configuration["Authentication:JwtBearer:Issuer"],
                    //ValidAudience = configuration["Authentication:JwtBearer:Audience"],
                    IssuerSigningKey = new RsaSecurityKey(rsa)
                };
            });

        //services.Configure<DbContextOptions<AuditLoggingDbContext>>(options =>
        //{
        //    options.UseSqlite(options => options.MigrationsAssembly(GetType().Assembly.FullName));
        //});

        services
            .AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("Default"));
            })
            .AddDbContext<TenantManagementDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("Default"));
            })
            .AddDbContext<AuditLoggingDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("Default"));
            })
            .AddDbContext<FeatureManagementDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("Default"));
            })
            .AddDbContext<PermissionManagementDbContext>(options =>
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