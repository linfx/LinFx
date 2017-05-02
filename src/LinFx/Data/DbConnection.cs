using System.Data;

namespace LinFx.Data
{
    public class DbConnection : IDbConnection
    {
        private IDbConnection connection;

        private DbConnectionFactory Factory;

        public DbConnection(DbConnectionFactory factory) => Factory = factory;

        public string ConnectionString { get => Factory.ConnectionString; set => Factory.ConnectionString = value; }

        public IDbConnection Connection => connection ?? (connection = DbProvider.ToDbConnection(ConnectionString, Factory.DbProvider));

        public int ConnectionTimeout => Connection.ConnectionTimeout;

        public string Database => Connection.Database;

        public ConnectionState State => Connection.State;

        public IDbTransaction BeginTransaction() => Connection.BeginTransaction();

        public IDbTransaction BeginTransaction(IsolationLevel il) => Connection.BeginTransaction(il);

        public void ChangeDatabase(string databaseName) => Connection.ChangeDatabase(databaseName);

        public IDbCommand CreateCommand() => Connection.CreateCommand();

        public void Close() => Connection.Close();

        public void Dispose() => Connection.Dispose();

        public void Open()
        {
            if (Connection.State == ConnectionState.Broken)
                Connection.Close();

            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }
        }

        public override string ToString()
        {
            return Factory.DbProvider.ToString();
        }
    }
}
