using Api.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Api.IntegrationTests.Repositories;

/// <summary>
/// Exercises <see cref="SensorCategoryRepository"/> against a real SQL Server LocalDB instance stood up
/// from SQLScripts/CreateTables.sql (seeded via SQLScripts/Sensor_Categories-Insert.sql).
/// </summary>
public class SensorCategoryRepositoryTests
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
    public async Task GetByIdAsync_ReturnsSensorCategory_ForSeededId()
    {
        await using var context = CreateContext();
        var repository = new SensorCategoryRepository(context);

        // Sensor_Categories-Insert.sql seeds Id 1 as Thermostat/C.
        var category = await repository.GetByIdAsync(1);

        Assert.NotNull(category);
        Assert.Equal("Thermostat", category!.SensorModel);
        Assert.Equal("C", category.MeasurementUnit);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_ForUnknownId()
    {
        await using var context = CreateContext();
        var repository = new SensorCategoryRepository(context);

        var category = await repository.GetByIdAsync(-1);

        Assert.Null(category);
    }
}
