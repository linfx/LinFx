using System.Reflection;

namespace LinFx.Extensions.Authorization;

/// <summary>
/// 上下文
/// </summary>
/// <param name="method"></param>
public class MethodInvocationAuthorizationContext(MethodInfo method)
{
    /// <summary>
    /// 方法
    /// </summary>
    public MethodInfo Method { get; } = method;
}
