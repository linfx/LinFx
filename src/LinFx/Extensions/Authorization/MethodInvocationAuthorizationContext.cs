using System.Reflection;

namespace LinFx.Extensions.Authorization;

public class MethodInvocationAuthorizationContext(MethodInfo method)
{
    public MethodInfo Method { get; } = method;
}
