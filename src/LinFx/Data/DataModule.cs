using LinFx.Extensions.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace LinFx.Data
{
    public class DataModule : Module
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>));
        }
    }
}
