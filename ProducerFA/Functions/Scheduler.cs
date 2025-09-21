using System;
using System.Text.Json;
using Domain.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ProducerFA.Services.Interfaces;

namespace ProducerFA.Functions
{
    public class Scheduler
    {
        private readonly ILogger _logger;
        private readonly IApiService _apiService;
        private readonly IProducerService _producerService;
        public Scheduler(ILoggerFactory loggerFactory, IApiService apiService, IProducerService producerService)
        {
            _logger = loggerFactory.CreateLogger<Scheduler>();
            _apiService = apiService;
            _producerService = producerService;
        }


        [Function("GetScheduledCalculations")]
        public void Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            // TODO: Implement pagingation here to start sending messages in batches.
            List<ScheduledCalcs> jobs = _apiService.RetrieveScheduledCalculations().Result;

            foreach (var job in jobs)
            {
                _producerService.SendMessageAsync(JsonSerializer.Serialize(job));
            }
            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
