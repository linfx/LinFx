using LinFx;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;


namespace IdentityService;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<Application>();
    }

    public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        await app.InitializeApplicationAsync();
    }
}