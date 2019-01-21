using System.Data;

namespace LinFx.Extensions.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateDbConnection();

        IDbConnection OpenDbConnection();
    }
}
