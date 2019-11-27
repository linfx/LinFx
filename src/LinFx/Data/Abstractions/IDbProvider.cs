using System.Data;

namespace LinFx.Data.Abstractions
{
    public interface IDbProvider
    {
        IDbConnection CreateConnection(string connectionString);
    }
}