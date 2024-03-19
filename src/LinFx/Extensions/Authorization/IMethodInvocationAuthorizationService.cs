namespace LinFx.Extensions.Authorization;

public interface IMethodInvocationAuthorizationService
{
    /// <summary>
    /// 权限校验
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task CheckAsync(MethodInvocationAuthorizationContext context);
}