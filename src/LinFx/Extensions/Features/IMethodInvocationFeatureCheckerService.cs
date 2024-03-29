namespace LinFx.Extensions.Features;

public interface IMethodInvocationFeatureCheckerService
{
    /// <summary>
    /// 特征校验
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    Task CheckAsync(MethodInvocationFeatureCheckerContext context);
}
