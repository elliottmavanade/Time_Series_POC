using Api.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Api.IntegrationTests.Repositories;

/// <summary>
/// Exercises <see cref="SensorReadingRepository"/> against a real SQL Server LocalDB instance stood up
/// from SQLScripts/CreateTables.sql (seeded via SQLScripts/Sensor_Readings-Insert.sql).
/// </summary>
public class SensorReadingRepositoryTests
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
    public async Task GetValuesAsync_ReturnsValues_WithinDateRange_ForMultipleSensorIds()
    {
        await using var context = CreateContext();
        var repository = new SensorReadingRepository(context);

        // Sensor_Readings-Insert.sql seeds sensors 1 and 2 with 24 hourly readings on 2025-09-16.
        var values = await repository.GetValuesAsync(
            new[] { 1, 2 },
            new DateTime(2025, 9, 16, 0, 0, 0),
            new DateTime(2025, 9, 16, 1, 0, 0));

        Assert.Equal(4, values.Count); // 2 sensors x 2 hourly readings (00:00 and 01:00) each.
    }

    [Fact]
    public async Task GetValuesAsync_ReturnsEmptyList_WhenNoReadingsInRange()
    {
        await using var context = CreateContext();
        var repository = new SensorReadingRepository(context);

        var values = await repository.GetValuesAsync(new[] { 1 }, new DateTime(1999, 1, 1), new DateTime(1999, 1, 2));

        Assert.Empty(values);
    }

    [Fact]
    public async Task AddAsync_PersistsNewReading_AndCanBeReadBack()
    {
        await using var context = CreateContext();
        var repository = new SensorReadingRepository(context);

        var dateCreated = new DateTime(2030, 1, 1, 12, 0, 0);
        var added = await repository.AddAsync(sensorId: 1, sensorValue: 999, dateCreated);

        Assert.True(added);

        var values = await repository.GetValuesAsync(new[] { 1 }, dateCreated, dateCreated);
        Assert.Contains(999, values);

        // Clean up so repeated test runs stay independent of leftover data.
        var inserted = await context.SensorReadings.SingleAsync(r => r.SensorId == 1 && r.SensorValue == 999 && r.DateCreated == dateCreated);
        context.SensorReadings.Remove(inserted);
        await context.SaveChangesAsync();
    }
}
