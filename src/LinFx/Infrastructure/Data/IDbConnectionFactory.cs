using System.Data;

namespace LinFx.Infrastructure.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateDbConnection();
        IDbConnection OpenDbConnection();
    }
}
