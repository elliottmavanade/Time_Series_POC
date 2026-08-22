using Api.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Api.IntegrationTests.Repositories;

/// <summary>
/// Exercises <see cref="ScheduledCalcsRepository"/> against a real SQL Server LocalDB instance stood up
/// from SQLScripts/CreateTables.sql (seeded via SQLScripts/Scheduled_Calcs-Insert.sql).
/// </summary>
public class ScheduledCalcsRepositoryTests
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
    public async Task GetAllAsync_ReturnsAllSeededScheduledCalcs()
    {
        await using var context = CreateContext();
        var repository = new ScheduledCalcsRepository(context);

        var scheduledCalcs = await repository.GetAllAsync();

        // Scheduled_Calcs-Insert.sql seeds exactly these 4 rows.
        Assert.Equal(4, scheduledCalcs.Count);
        Assert.Contains(scheduledCalcs, sc => sc.Name == "Calc_Curtin:_Avg_Temp_Day" && sc.SensorId == 7);
        Assert.Contains(scheduledCalcs, sc => sc.Name == "Calc_Curtin:_Sum_Energy_Week" && sc.SensorId == 8);
    }
}
