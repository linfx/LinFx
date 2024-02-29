using LinFx;

namespace TenantManagementService;

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
