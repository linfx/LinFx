namespace LinFx.Domain.Models.Auditing
{
    public interface IAuditSerializer
    {
        string Serialize(object obj);
    }
}