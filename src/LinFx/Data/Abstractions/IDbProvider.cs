using System.Data;

namespace LinFx.Extensions.Data
{
    public interface IDbProvider
    {
        IDbConnection CreateConnection(string connectionString);
    }
}