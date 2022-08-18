using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace TenantManagementService;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApplication<TenantManagementServiceModule>();
    }

    public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        await app.InitializeApplicationAsync();
    }
}
