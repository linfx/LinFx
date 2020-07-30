using MassTransit;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace LinFx.Extensions.EventBus.MassTransit
{
    /// <summary>
    /// Mq后台服务
    /// </summary>
    public class MassTransitHostedService : IHostedService
    {
        private readonly IBusControl _busControl;

        public MassTransitHostedService(IBusControl busControl)
        {
            _busControl = busControl;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _busControl.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _busControl.StopAsync(cancellationToken);
        }
    }
}
