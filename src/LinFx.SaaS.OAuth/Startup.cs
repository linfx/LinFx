using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using LinFx.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;

namespace LinFx.SaaS.OAuth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthorization(auth =>
            //{
            //    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
            //        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            //        .RequireAuthenticatedUser()
            //        .Build());
            //});

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.Authority = Configuration["jwt:authority"];
                o.Audience = Configuration["jwt:audience"];
                //o.Events = new JwtBearerEvents()
                //{
                //    OnAuthenticationFailed = c =>
                //    {
                //        c.NoResult();

                //        c.Response.StatusCode = 500;
                //        c.Response.ContentType = "text/plain";
                //        if (Environment.IsDevelopment())
                //        {
                //            // Debug only, in production do not share exceptions with the remote host.
                //            return c.Response.WriteAsync(c.Exception.ToString());
                //        }
                //        return c.Response.WriteAsync("An error occurred processing your authentication.");
                //    }
                //};
            });

            services.AddMvc();
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //var option = new JwtTokenOptions();

            //var tokenValidationParameters = new TokenValidationParameters
            //{
            //    // The signing key must match!
            //    ValidateIssuerSigningKey = true,
            //    IssuerSigningKey = option.Key,

            //    // Validate the JWT Issuer (iss) claim
            //    ValidateIssuer = true,
            //    //ValidIssuer = option.i,

            //    // Validate the JWT Audience (aud) claim
            //    ValidateAudience = true,
            //    ValidAudience = "ExampleAudience",

            //    // Validate the token expiry
            //    ValidateLifetime = true,

            //    // If you want to allow a certain amount of clock drift, set that here:
            //    ClockSkew = TimeSpan.Zero
            //};

            ////app.UseJwtBearerAuthentication(new JwtBearerOptions
            ////{
            ////    AutomaticAuthenticate = true,
            ////    AutomaticChallenge = true,
            ////    TokenValidationParameters = tokenValidationParameters
            ////});

            app.UseAuthentication();

            // [Authorize] would usually handle this
            app.Use(async (context, next) =>
            {
                // Use this if there are multiple authentication schemes
                // var user = await context.Authentication.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

                var user = context.User; // We can do this because of there's only a single authentication scheme
                if (user?.Identity?.IsAuthenticated ?? false)
                {
                    await next();
                }
                else
                {
                    await context.ChallengeAsync();
                }
            });

            app.UseMvc();
        }
    }
}
