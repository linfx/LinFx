using System.Threading.Tasks;

namespace LinFx.Security.Authorization
{
    public interface IMethodInvocationAuthorizationService
    {
        Task CheckAsync(MethodInvocationAuthorizationContext context);
    }
}