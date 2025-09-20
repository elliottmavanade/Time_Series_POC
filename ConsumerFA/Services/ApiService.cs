using Api.Infrastructure;
using ConsumerFA.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
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

        public async Task<Dictionary<int, List<int>>> GetSensorRelationships(int parentId)
        {
            var route = ActionRoutes.GetSensorRelationships.Replace("{parentId}", parentId.ToString());
            var url = $"http://localhost:5170/{ApiRoutes.Dashboard}/{route}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var jobs = JsonSerializer.Deserialize<Dictionary<int, List<int>>>(json, options);

            return jobs ?? new Dictionary<int, List<int>>();
        }
    }
}
