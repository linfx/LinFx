using System.Data;

namespace LinFx.Infrastructure.Data
{
    public interface IDbProvider
    {
        IDbConnection CreateConnection(string connectionString);
    }
}