using System.Net;
using ConsumerFA.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace ConsumerFA.Tests.Services;

public class ApiServiceGetSensorTests
{
    private static ApiService CreateService(string responseJson)
    {
        var handler = new StubHttpMessageHandler(responseJson);
        var httpClient = new HttpClient(handler);
        return new ApiService(NullLogger<ApiService>.Instance, httpClient);
    }

    [Fact]
    public async Task GetSensor_ParsesMetadataJson_IntoAggregationAndTimespan()
    {
        // Api now returns the raw scaffolded Domain.Entities.Sensor shape: Metadata is an un-parsed
        // JSON string rather than pre-computed Aggregation/Timespan fields.
        var responseJson = """
            {
                "id": 6,
                "sensorModelId": 2,
                "location": "Curtin:B01",
                "metadata": "{\"Aggregation\":\"Sum\",\"Timespan\":1440}"
            }
            """;

        var service = CreateService(responseJson);

        var sensor = await service.GetSensor(6);

        Assert.Equal(6, sensor.Id);
        Assert.Equal(2, sensor.Sensor_Model_Id);
        Assert.Equal("Curtin:B01", sensor.Location);
        Assert.Equal(Domain.Enums.AggregationType.Sum, sensor.Aggregation);
        Assert.Equal(1440, sensor.Timespan);
    }

    [Fact]
    public async Task GetSensor_ReturnsNullAggregationAndTimespan_WhenMetadataIsNull()
    {
        var responseJson = """
            {
                "id": 1,
                "sensorModelId": 1,
                "location": "Curtin:B01",
                "metadata": null
            }
            """;

        var service = CreateService(responseJson);

        var sensor = await service.GetSensor(1);

        Assert.Null(sensor.Aggregation);
        Assert.Null(sensor.Timespan);
    }

    private sealed class StubHttpMessageHandler(string responseJson) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseJson)
            };
            return Task.FromResult(response);
        }
    }
}
