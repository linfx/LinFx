using System.Threading.Tasks;

namespace LinFx.Extensions.EventBus.Abstractions
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}