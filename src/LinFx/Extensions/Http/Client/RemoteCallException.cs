using LinFx.Extensions.ExceptionHandling;
using System.Runtime.Serialization;

namespace LinFx.Extensions.Http.Client;

/// <summary>
/// 远程调用异常
/// </summary>
[Serializable]
public class RemoteCallException : Exception, IHasErrorCode, IHasHttpStatusCode
{
    public int HttpStatusCode { get; set; }

    public string Code => Error?.Code;

    public string Details => Error?.Details;

    public RemoteServiceErrorInfo Error { get; set; }

    public RemoteCallException() { }

    public RemoteCallException(string message, Exception innerException = null) : base(message, innerException) { }

    public RemoteCallException(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context) { }

    public RemoteCallException(RemoteServiceErrorInfo error, Exception innerException = null) : base(error.Message, innerException)
    {
        Error = error;

        if (error.Data != null)
        {
            foreach (var dataKey in error.Data.Keys)
            {
                Data[dataKey] = error.Data[dataKey];
            }
        }
    }
}
