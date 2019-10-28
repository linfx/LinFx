using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LinFx.Modules
{
    public interface IModule
    {
        void ConfigureServices(IServiceCollection services);

        void Configure(IApplicationBuilder app, IHostEnvironment env);
    }
}
