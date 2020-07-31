using System.Reflection;

namespace LinFx.Extensions.Authorization
{
    public class MethodInvocationAuthorizationContext
    {
        public MethodInfo Method { get; }

        public MethodInvocationAuthorizationContext(MethodInfo method)
        {
            Method = method;
        }
    }
}