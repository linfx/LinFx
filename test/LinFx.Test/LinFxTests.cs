using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LinFx.Test
{
    public class LinFxTests
    {
        [Fact]
        public void Startup()
        {
            var services = new ServiceCollection();
            services.AddLinFx();
            var container = services.BuildServiceProvider();

            var result = Result.Failed("222");

            if(!result)
            {

            }
            else
            {

            }
        }
    }
}
