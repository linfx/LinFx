namespace LinFx.Extensions.Exceptions
{
    public interface IHasHttpStatusCode
    {
        int HttpStatusCode { get; }
    }
}