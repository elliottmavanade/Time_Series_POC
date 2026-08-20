using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Domain.IntegrationTests;

/// <summary>
/// Smoke tests confirming the scaffolded <see cref="TimeSeriesPocDbContext"/> can connect to and
/// query the LocalDB instance stood up from SQLScripts/CreateTables.sql.
/// </summary>
public class TimeSeriesPocDbContextTests
{
    private static string GetConnectionString()
    {
        // Read from Api/appsettings.json so the connection string has a single source of
        // truth instead of being duplicated as a literal in this test project.
        var apiProjectDirectory = FindApiProjectDirectory(AppContext.BaseDirectory);

        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiProjectDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        return configuration.GetConnectionString("TimeSeriesPoc")
            ?? throw new InvalidOperationException(
                "Missing 'ConnectionStrings:TimeSeriesPoc' in Api/appsettings.json.");
    }

    private static string FindApiProjectDirectory(string startDirectory)
    {
        for (var directory = new DirectoryInfo(startDirectory); directory is not null; directory = directory.Parent!)
        {
            var candidate = Path.Combine(directory.FullName, "Api");
            if (File.Exists(Path.Combine(candidate, "appsettings.json")))
            {
                return candidate;
            }
        }

        throw new DirectoryNotFoundException(
            $"Could not locate the Api project (with appsettings.json) above '{startDirectory}'.");
    }

    private static TimeSeriesPocDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<TimeSeriesPocDbContext>()
            .UseSqlServer(GetConnectionString())
            .Options;

        return new TimeSeriesPocDbContext(options);
    }

    [Fact]
    public async Task CanConnect_ToLocalDbInstance()
    {
        await using var context = CreateContext();

        var canConnect = await context.Database.CanConnectAsync();

        Assert.True(canConnect);
    }

    [Theory]
    [InlineData(typeof(SensorCategory))]
    [InlineData(typeof(Sensor))]
    [InlineData(typeof(SensorReading))]
    [InlineData(typeof(ScheduledCalc))]
    public async Task CanQuery_EveryScaffoldedTable(Type _)
    {
        await using var context = CreateContext();

        // Executes a lightweight COUNT query against every table scaffolded from
        // SQLScripts/CreateTables.sql to confirm the DbContext maps them correctly.
        await context.SensorCategories.CountAsync();
        await context.Sensors.CountAsync();
        await context.SensorReadings.CountAsync();
        await context.ScheduledCalcs.CountAsync();
    }

    [Fact]
    public async Task CanQuery_SensorCalcRelationships_ViaSkipNavigations()
    {
        // Sensor_Calc_Relationships is a pure join table (composite key of the two FKs only),
        // so EF Core's scaffolder models it as skip navigations on Sensor rather than its own
        // entity class. Querying through those navigations confirms the join table is mapped.
        await using var context = CreateContext();

        var sensors = await context.Sensors
            .Include(s => s.SensorParents)
            .Include(s => s.SensorChildren)
            .ToListAsync();

        Assert.NotNull(sensors);
    }
}
