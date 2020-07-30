namespace LinFx.Extensions.Auditing
{
    public interface IAuditSerializer
    {
        string Serialize(object obj);
    }
}