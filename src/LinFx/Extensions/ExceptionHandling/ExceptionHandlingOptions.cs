namespace LinFx.Extensions.ExceptionHandling;

/// <summary>
/// 异常处理选项
/// </summary>
public class ExceptionHandlingOptions
{
    public bool SendExceptionsDetailsToClients { get; set; } = false;

    public bool SendStackTraceToClients { get; set; } = true;
}
