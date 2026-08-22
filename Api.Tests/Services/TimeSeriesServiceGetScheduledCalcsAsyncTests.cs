using Api.Repositories.Interfaces;
using Api.Services;
using Domain.Entities;
using Moq;

namespace Api.Tests.Services;

public class TimeSeriesServiceGetScheduledCalcsAsyncTests
{
    [Fact]
    public async Task GetScheduledCalcsAsync_MapsEntitiesToWireContract()
    {
        var entities = new List<ScheduledCalc>
        {
            new() { Id = 1, Name = "Calc_Curtin:_Avg_Temp_Day", SensorId = 7 },
            new() { Id = 2, Name = "Calc_Curtin:B01_Sum_Energy_Day", SensorId = 6 },
        };

        var repository = new Mock<IScheduledCalcsRepository>();
        repository.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);

        var service = new TimeSeriesService(repository.Object, new Mock<ISensorReadingRepository>().Object);

        var result = await service.GetScheduledCalcsAsync();

        Assert.Collection(result,
            sc => { Assert.Equal("Calc_Curtin:_Avg_Temp_Day", sc.Name); Assert.Equal(7, sc.Sensor_Id); },
            sc => { Assert.Equal("Calc_Curtin:B01_Sum_Energy_Day", sc.Name); Assert.Equal(6, sc.Sensor_Id); });
    }

    [Fact]
    public async Task GetScheduledCalcsAsync_ReturnsEmptyList_WhenRepositoryReturnsNone()
    {
        var repository = new Mock<IScheduledCalcsRepository>();
        repository.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<ScheduledCalc>());

        var service = new TimeSeriesService(repository.Object, new Mock<ISensorReadingRepository>().Object);

        var result = await service.GetScheduledCalcsAsync();

        Assert.Empty(result);
    }
}
