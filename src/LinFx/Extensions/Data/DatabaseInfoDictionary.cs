namespace LinFx.Extensions.Data;

public class DatabaseInfoDictionary : Dictionary<string, DatabaseInfo>
{
    private Dictionary<string, DatabaseInfo> ConnectionIndex { get; set; } = [];

    public DatabaseInfo? GetMappedDatabaseOrNull(string connectionStringName) => ConnectionIndex.GetOrDefault(connectionStringName);

    public DatabaseInfoDictionary Configure(string databaseName, Action<DatabaseInfo> configureAction)
    {
        var databaseInfo = this.GetOrAdd(databaseName, () => new DatabaseInfo(databaseName));
        configureAction(databaseInfo);
        return this;
    }

    /// <summary>
    /// This method should be called if this dictionary changes.
    /// It refreshes indexes for quick access to the connection informations.
    /// </summary>
    public void RefreshIndexes()
    {
        ConnectionIndex = new Dictionary<string, DatabaseInfo>();

        foreach (var databaseInfo in Values)
        {
            foreach (var mappedConnection in databaseInfo.MappedConnections)
            {
                if (ConnectionIndex.ContainsKey(mappedConnection))
                    throw new Exception($"A connection name can not map to multiple databases: {mappedConnection}.");

                ConnectionIndex[mappedConnection] = databaseInfo;
            }
        }
    }
}
