using Api.Models.DTOs;
using Api.Repositories.Interfaces;
using Api.Services;
using Domain.Entities;
using Moq;

namespace Api.Tests.Services;

public class TimeSeriesServiceSensorReadingTests
{
    [Fact]
    public async Task GetTimeSeriesDataAsync_SingleId_DelegatesToRepositoryWithComputedDateRange()
    {
        var scheduledCalcsRepository = new Mock<IScheduledCalcsRepository>();
        var sensorReadingRepository = new Mock<ISensorReadingRepository>();
        sensorReadingRepository
            .Setup(r => r.GetValuesAsync(It.Is<IEnumerable<int>>(ids => ids.SequenceEqual(new[] { 1 })), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<int> { 21, 22, 23 });

        var service = new TimeSeriesService(scheduledCalcsRepository.Object, sensorReadingRepository.Object);

        var result = await service.GetTimeSeriesDataAsync(1, 60);

        Assert.Equal(new List<int> { 21, 22, 23 }, result);
    }

    [Fact]
    public async Task GetTimeSeriesDataAsync_MultipleIds_DelegatesToRepositoryWithAllIds()
    {
        var scheduledCalcsRepository = new Mock<IScheduledCalcsRepository>();
        var sensorReadingRepository = new Mock<ISensorReadingRepository>();
        sensorReadingRepository
            .Setup(r => r.GetValuesAsync(It.Is<IEnumerable<int>>(ids => ids.SequenceEqual(new[] { 3, 4, 5 })), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
            .ReturnsAsync(new List<int> { 2, 3, 4 });

        var service = new TimeSeriesService(scheduledCalcsRepository.Object, sensorReadingRepository.Object);

        var result = await service.GetTimeSeriesDataAsync(new[] { 3, 4, 5 }, 1440);

        Assert.Equal(new List<int> { 2, 3, 4 }, result);
    }

    [Fact]
    public async Task AddTimeSeriesResultAsync_DelegatesToRepository_AndReturnsItsResult()
    {
        var scheduledCalcsRepository = new Mock<IScheduledCalcsRepository>();
        var sensorReadingRepository = new Mock<ISensorReadingRepository>();
        sensorReadingRepository
            .Setup(r => r.AddAsync(3, 42, It.IsAny<DateTime>()))
            .ReturnsAsync(true);

        var service = new TimeSeriesService(scheduledCalcsRepository.Object, sensorReadingRepository.Object);

        var result = await service.AddTimeSeriesResultAsync(new TimeSeriesResultDTO { SensorId = 3, SensorValue = 42 });

        Assert.True(result);
    }

    [Fact]
    public async Task AddTimeSeriesResultAsync_ReturnsFalse_WhenRepositoryFails()
    {
        var scheduledCalcsRepository = new Mock<IScheduledCalcsRepository>();
        var sensorReadingRepository = new Mock<ISensorReadingRepository>();
        sensorReadingRepository
            .Setup(r => r.AddAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTime>()))
            .ReturnsAsync(false);

        var service = new TimeSeriesService(scheduledCalcsRepository.Object, sensorReadingRepository.Object);

        var result = await service.AddTimeSeriesResultAsync(new TimeSeriesResultDTO { SensorId = 3, SensorValue = 42 });

        Assert.False(result);
    }
}
