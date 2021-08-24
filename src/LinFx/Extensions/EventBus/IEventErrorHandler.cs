using System.Threading.Tasks;

namespace LinFx.Extensions.EventBus
{
    public interface IEventErrorHandler
    {
        Task HandleAsync(EventExecutionErrorContext context);
    }
}
