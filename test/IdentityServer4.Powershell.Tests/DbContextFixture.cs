using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace IdentityServer4.Powershell.Tests
{
    public class DbContextFixture
    {
        Dictionary<string, SqliteConnection> _connections;
        
        public DbContextFixture()
        {
            _connections = new Dictionary<string, SqliteConnection>();
        }

        internal ConfigurationDbContext GetContext(string ctxName)
        {
            var connectionExists = _connections.TryGetValue(ctxName, out var connection);
            if (!connectionExists) {
                connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();
                _connections.Add(ctxName, connection);
            }

            var ctxOptions = new DbContextOptionsBuilder<ConfigurationDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new ConfigurationDbContext(ctxOptions, new ConfigurationStoreOptions());
            context.Database.EnsureCreated();

            return context;
        }
    }
}
