namespace LinFx.Security.Users;

public interface ICurrentClient
{
    string Id { get; }

    bool IsAuthenticated { get; }
}
