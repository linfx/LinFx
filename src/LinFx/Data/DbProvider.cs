using LinFx.Data.Abstractions;
using System.Data;

namespace LinFx.Data
{
    public static class DbProvider
    {
        public static IDbConnection ToDbConnection(string connectionString, IDbProvider provider)
        {
            return provider.CreateConnection(connectionString);
        }
    }
}