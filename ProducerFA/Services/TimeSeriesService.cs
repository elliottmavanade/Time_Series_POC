using Api.Infrastructure;
using Domain.Models;
using Microsoft.Extensions.Logging;
using ProducerFA.Infrastructure;
using ProducerFA.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProducerFA.Services
{
    public class TimeSeriesService : ITimeSeriesService
    {
        private readonly ILogger<TimeSeriesService> _logger;
        private readonly HttpClient _httpClient;

        public TimeSeriesService(ILogger<TimeSeriesService> logger, HttpClient httpClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<ScheduledCalcs>> RetrieveScheduledCalculations()
        {
            var url = $"http://localhost:5170/{ApiRoutes.Dashboard}/{ActionRoutes.GetScheduledRuns}";
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var jobs = JsonSerializer.Deserialize<List<ScheduledCalcs>>(json);

            return jobs ?? new List<ScheduledCalcs>();
        }
    }
}
