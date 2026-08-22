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
    [InlineData(typeof(SensorCalcRelationship))]
    public async Task CanQuery_EveryScaffoldedTable(Type _)
    {
        await using var context = CreateContext();

        // Executes a lightweight COUNT query against every table scaffolded from
        // SQLScripts/CreateTables.sql to confirm the DbContext maps them correctly.
        await context.SensorCategories.CountAsync();
        await context.Sensors.CountAsync();
        await context.SensorReadings.CountAsync();
        await context.ScheduledCalcs.CountAsync();
        await context.SensorCalcRelationships.CountAsync();
    }

    [Fact]
    public async Task CanQuery_SensorCalcRelationships_AsExplicitJoinEntity()
    {
        // Sensor_Calc_Relationships is modeled as an explicit SensorCalcRelationship join entity
        // (ParentSensorId/ChildSensorId), not an EF skip-navigation many-to-many, so a parent sensor's
        // children can be queried directly against it.
        await using var context = CreateContext();

        var relationships = await context.SensorCalcRelationships.ToListAsync();

        Assert.NotEmpty(relationships);
    }
}
