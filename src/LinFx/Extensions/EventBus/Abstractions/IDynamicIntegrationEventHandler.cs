using System.Threading.Tasks;

namespace LinFx.Extensions.EventBus
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}