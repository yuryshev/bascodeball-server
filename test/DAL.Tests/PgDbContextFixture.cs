using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Xunit;

namespace DAL.Tests;
[CollectionDefinition("Pg db collection", DisableParallelization = true)]
public class PgDbCollection : ICollectionFixture<PgDbContextFixture>
{
}

public class PgDbContextFixture : IDisposable
{
    private readonly DbContextOptionsBuilder<PgDbContext> _brokenContextBuilder;
    private readonly DbContextOptionsBuilder<PgDbContext> _pgDbContextBuilder;
    private readonly PgDbContext _dbContextForDeleteDb;

    public PgDbContextFixture()
    {
        // Prepare broken DB context build which refers to non-existing DB.
        var nonExistingDbConnectionStringBuilder =
            new NpgsqlConnectionStringBuilder(ConfigManager.Configuration.GetConnectionString("TestPostgresConnection"))
            {
                Database = "PgDb" + Guid.NewGuid(),
            };

        this._brokenContextBuilder = new DbContextOptionsBuilder<PgDbContext>()
            .UseNpgsql(nonExistingDbConnectionStringBuilder.ConnectionString);

        // Prepare normal DB context builder and ensure DB is created.
        var connectionStringBuilder = new NpgsqlConnectionStringBuilder(ConfigManager.Configuration.GetConnectionString("TestPostgresConnection"))
        {
            Database = "PgDb-" + Guid.NewGuid(),
        };

        var builder = new DbContextOptionsBuilder<PgDbContext>()
            .UseNpgsql(connectionStringBuilder.ConnectionString, o => o.SetPostgresVersion(13, 0));

        this._pgDbContextBuilder = builder;

        var dbContext = new PgDbContext(this._pgDbContextBuilder.Options);
        dbContext.Database.EnsureCreated();

        this._dbContextForDeleteDb = dbContext;
    }

    public PgDbContext GetPgDbContext()
    {
        return new PgDbContext(this._pgDbContextBuilder.Options);
    }

    public PgDbContext GetBrokenPgDbContext()
    {
        return new PgDbContext(this._brokenContextBuilder.Options);
    }

    /// <inheritdoc />
    public void Dispose()
    {
        this._dbContextForDeleteDb?.Database.EnsureDeleted();
        this._dbContextForDeleteDb?.Dispose();
    }
}
