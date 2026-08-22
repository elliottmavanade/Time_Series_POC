using Api.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Api.IntegrationTests.Repositories;

/// <summary>
/// Exercises <see cref="SensorCalcRelationshipRepository"/> against a real SQL Server LocalDB instance
/// stood up from SQLScripts/CreateTables.sql (seeded via SQLScripts/Sensor_Calc_Relationships-Insert.sql).
/// </summary>
public class SensorCalcRelationshipRepositoryTests
{
    private static string GetConnectionString()
    {
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
    public async Task GetChildSensorIdsAsync_ReturnsAllChildren_ForParentWithMultipleChildren()
    {
        await using var context = CreateContext();
        var repository = new SensorCalcRelationshipRepository(context);

        // Sensor_Calc_Relationships-Insert.sql: parent 9 -> children 3, 4, 5.
        var childIds = await repository.GetChildSensorIdsAsync(9);

        Assert.Equal(new[] { 3, 4, 5 }, childIds.OrderBy(id => id));
    }

    [Fact]
    public async Task GetChildSensorIdsAsync_ReturnsEmptyList_ForParentWithNoChildren()
    {
        await using var context = CreateContext();
        var repository = new SensorCalcRelationshipRepository(context);

        var childIds = await repository.GetChildSensorIdsAsync(1);

        Assert.Empty(childIds);
    }
}
