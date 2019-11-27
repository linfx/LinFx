using LinFx.Data.Abstractions;
using System;
using System.Data;

namespace LinFx.Data
{
    public class DbConnectionFactory : IDbConnectionFactory, IDisposable
    {
        private DbConnection _connection;

        private DbConnection Connection => _connection ?? (_connection = new DbConnection(this));

        public IDbProvider DbProvider { get; }

        public string ConnectionString { get; set; }

        public DbConnectionFactory(string connectionString, IDbProvider provider)
        {
            ConnectionString = connectionString;
            DbProvider = provider;
        }

        public virtual IDbConnection OpenDbConnection()
        {
            var connection = CreateDbConnection();
            connection.Open();
            return connection;
        }

        public virtual IDbConnection CreateDbConnection()
        {
            if (ConnectionString == null)
                throw new ArgumentNullException("ConnectionString");

            var connection = new DbConnection(this);
            return connection;
        }

        public void Dispose()
        {
            Connection.Dispose();
        }
    }

    public static class DbConnectionFactoryExtensions
    {
        /// <summary>
        /// Alias for OpenDbConnection
        /// </summary>
        public static IDbConnection Open(this IDbConnectionFactory factory)
        {
            return factory.OpenDbConnection();
        }

        /// <summary>
        /// Alias for CreateDbConnection
        /// </summary>
        public static IDbConnection Create(this IDbConnectionFactory factory)
        {
            return factory.CreateDbConnection();
        }
    }
}