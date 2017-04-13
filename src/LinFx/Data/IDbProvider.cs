using System.Data;

namespace LinFx.Data
{
    public interface IDbProvider
    {
        IDbConnection CreateConnection(string connectionString);
    }
}
