using System.Data;

namespace LinFx.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateDbConnection();
        IDbConnection OpenDbConnection();
    }
}
