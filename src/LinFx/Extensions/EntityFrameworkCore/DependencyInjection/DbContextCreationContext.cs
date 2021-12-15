using System;
using System.Data.Common;
using System.Threading;

namespace LinFx.Extensions.EntityFrameworkCore.DependencyInjection
{
    public class DbContextCreationContext
    {
        public static DbContextCreationContext Current => _current.Value;
        private static readonly AsyncLocal<DbContextCreationContext> _current = new();

        public string ConnectionStringName { get; }

        public string ConnectionString { get; }

        public DbConnection ExistingConnection { get; internal set; }

        public DbContextCreationContext(string connectionStringName, string connectionString)
        {
            ConnectionStringName = connectionStringName;
            ConnectionString = connectionString;
        }

        public static IDisposable Use(DbContextCreationContext context)
        {
            var previousValue = Current;
            _current.Value = context;
            return new DisposeAction(() => _current.Value = previousValue);
        }
    }
}