using LinFx.Extensions.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

namespace IdentityService.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IdentityDbContext>(option =>
            {
                option.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly(typeof(Startup).Assembly.GetName().Name));
            });

            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity Service Api", Version = "v1" });
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "LinFx.Extensions.Identity.xml"), true);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "LinFx.Extensions.Identity.HttpApi.xml"), true);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "LinFx.Extensions.Account.xml"), true);
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "LinFx.Extensions.Account.HttpApi.xml"), true);
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
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity Service Api");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}