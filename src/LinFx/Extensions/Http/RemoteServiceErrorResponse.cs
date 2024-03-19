namespace LinFx.Extensions.Http;

public class RemoteServiceErrorResponse(RemoteServiceErrorInfo error)
{
    public RemoteServiceErrorInfo Error { get; set; } = error;
}
