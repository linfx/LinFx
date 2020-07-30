using System.Reflection;

namespace LinFx.Security.Authorization
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