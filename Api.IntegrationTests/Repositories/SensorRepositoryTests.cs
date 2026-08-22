using Api.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Api.IntegrationTests.Repositories;

/// <summary>
/// Exercises <see cref="SensorRepository"/> against a real SQL Server LocalDB instance stood up from
/// SQLScripts/CreateTables.sql (seeded via SQLScripts/Sensors-Insert.sql).
/// </summary>
public class SensorRepositoryTests
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
    public async Task GetByIdAsync_ReturnsSensor_WithMetadata_ForSeededSensorWithMetadata()
    {
        await using var context = CreateContext();
        var repository = new SensorRepository(context);

        // Sensor Id 6 is seeded (Sensors-Insert.sql) as Sensor_Model_Id 2, Curtin:B01, with metadata.
        var sensor = await repository.GetByIdAsync(6);

        Assert.NotNull(sensor);
        Assert.Equal(2, sensor!.SensorModelId);
        Assert.Equal("Curtin:B01", sensor.Location);
        Assert.NotNull(sensor.Metadata);
        Assert.Contains("Sum", sensor.Metadata);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsSensor_WithNullMetadata_ForSeededSensorWithoutMetadata()
    {
        await using var context = CreateContext();
        var repository = new SensorRepository(context);

        // Sensor Id 1 is seeded without a Metadata value.
        var sensor = await repository.GetByIdAsync(1);

        Assert.NotNull(sensor);
        Assert.Null(sensor!.Metadata);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_ForUnknownSensorId()
    {
        await using var context = CreateContext();
        var repository = new SensorRepository(context);

        var sensor = await repository.GetByIdAsync(-1);

        Assert.Null(sensor);
    }
}
