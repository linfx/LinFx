using System.Data;

namespace LinFx.Data.Abstractions
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateDbConnection();

        IDbConnection OpenDbConnection();
    }
}
