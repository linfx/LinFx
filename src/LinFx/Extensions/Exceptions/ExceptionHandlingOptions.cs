namespace LinFx.Extensions.Exceptions;

public class ExceptionHandlingOptions
{
    public bool SendExceptionsDetailsToClients { get; set; } = false;

    public bool SendStackTraceToClients { get; set; } = true;
}
