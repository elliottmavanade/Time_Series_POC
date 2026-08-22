using Api.Repositories.Interfaces;
using Api.Services;
using Moq;

namespace Api.Tests.Services;

public class SensorServiceGetSensorRelationshipsAsyncTests
{
    [Fact]
    public async Task GetSensorRelationshipsAsync_ReturnsParentIdAndChildIds_FromRepository()
    {
        var childIds = new List<int> { 3, 4, 5 };

        var sensorRepository = new Mock<ISensorRepository>();
        var relationshipRepository = new Mock<ISensorCalcRelationshipRepository>();
        relationshipRepository.Setup(r => r.GetChildSensorIdsAsync(9)).ReturnsAsync(childIds);

        var service = new SensorService(sensorRepository.Object, relationshipRepository.Object);

        var result = await service.GetSensorRelationshipsAsync(9);

        Assert.Equal(9, result.Item1);
        Assert.Equal(childIds, result.Item2);
    }

    [Fact]
    public async Task GetSensorRelationshipsAsync_ReturnsEmptyChildIds_WhenParentHasNoChildren()
    {
        var sensorRepository = new Mock<ISensorRepository>();
        var relationshipRepository = new Mock<ISensorCalcRelationshipRepository>();
        relationshipRepository.Setup(r => r.GetChildSensorIdsAsync(It.IsAny<int>())).ReturnsAsync(new List<int>());

        var service = new SensorService(sensorRepository.Object, relationshipRepository.Object);

        var result = await service.GetSensorRelationshipsAsync(1);

        Assert.Equal(1, result.Item1);
        Assert.Empty(result.Item2);
    }
}
