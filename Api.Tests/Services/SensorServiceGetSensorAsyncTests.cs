using Api.Repositories.Interfaces;
using Api.Services;
using Domain.Entities;
using Moq;

namespace Api.Tests.Services;

public class SensorServiceGetSensorAsyncTests
{
    [Fact]
    public async Task GetSensorAsync_ReturnsSensor_WhenRepositoryFindsIt()
    {
        var expected = new Sensor { Id = 6, SensorModelId = 2, Location = "Curtin:B01", Metadata = "{\"Aggregation\":\"Sum\",\"Timespan\":1440}" };

        var repository = new Mock<ISensorRepository>();
        repository.Setup(r => r.GetByIdAsync(6)).ReturnsAsync(expected);

        var service = new SensorService(repository.Object, new Mock<ISensorCalcRelationshipRepository>().Object);

        var result = await service.GetSensorAsync(6);

        Assert.Same(expected, result);
    }

    [Fact]
    public async Task GetSensorAsync_ReturnsNull_WhenRepositoryFindsNothing()
    {
        var repository = new Mock<ISensorRepository>();
        repository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Sensor?)null);

        var service = new SensorService(repository.Object, new Mock<ISensorCalcRelationshipRepository>().Object);

        var result = await service.GetSensorAsync(999);

        Assert.Null(result);
    }
}
