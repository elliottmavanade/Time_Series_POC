using Api.Infrastructure;
using ConsumerFA.Services.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Azure.Amqp.Framing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsumerFA.Services
{
    public class ApiService : IApiService
    {
        private readonly ILogger<ApiService> _logger;
        private readonly HttpClient _httpClient;

        public ApiService(ILogger<ApiService> logger, HttpClient httpClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<Tuple<int, List<int>>> GetSensorRelationships(int parentId)
        {
            var route = ActionRoutes.GetSensorRelationships.Replace("{parentId}", parentId.ToString());
            var url = $"http://localhost:5170/{ApiRoutes.Sensor}/{route}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var jobs = JsonSerializer.Deserialize<Tuple<int, List<int>>>(json, options);

            return jobs ?? throw new InvalidDataException($"Unable to deserialize Sensor Relationships, {json}");
        }

        public async Task<Sensor> GetSensor(int sensorId)
        {
            var route = ActionRoutes.GetSensor.Replace("{sensorId}", sensorId.ToString());
            var url = $"http://localhost:5170/{ApiRoutes.Sensor}/{route}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var sensor = JsonSerializer.Deserialize<Sensor>(json, options);

            return sensor ?? throw new InvalidDataException($"Unable to deserialize Sensor, {json}");
        }

        public async Task<List<int>> GetTimeSeriesData(List<int> childIds, int timespan)
        {
            var url = $"http://localhost:5170/{ApiRoutes.TimeSeries}/{ActionRoutes.GetTimeSeriesDataByIds}";

            var data = new
            {
                ChildIds = childIds,
                Timespan = timespan
            };

            var jsonPayload = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<List<int>>(jsonResponse, options);

            return result;
        }

        public async Task<bool> AddCalculatedResult(int sensorId, int calculatedResult)
        {
            var url = $"http://localhost:5170/{ApiRoutes.TimeSeries}/{ActionRoutes.AddTimeSeriesResult}";

            var data = new
            {
                SensorId = sensorId,
                SensorValue = calculatedResult
            };

            var jsonPayload = JsonSerializer.Serialize(data);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            return response.IsSuccessStatusCode;

        }
    }
}
