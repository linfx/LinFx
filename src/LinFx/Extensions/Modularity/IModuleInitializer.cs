using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LinFx.Extensions.Modules
{
    /// <summary>
    /// 模块初始化
    /// </summary>
    public interface IModuleInitializer
    {
        void ConfigureServices(IServiceCollection services);

        void Configure(IApplicationBuilder app, IHostEnvironment env);
    }
}
