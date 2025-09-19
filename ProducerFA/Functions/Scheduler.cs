using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ProducerFA.Services.Interfaces;

namespace ProducerFA.Functions
{
    public class Scheduler
    {
        private readonly ILogger _logger;
        private readonly ITimeSeriesService _timeSeriesService;
        public Scheduler(ILoggerFactory loggerFactory, ITimeSeriesService timeSeriesService)
        {
            _logger = loggerFactory.CreateLogger<Scheduler>();
            _timeSeriesService = timeSeriesService;
        }

        [Function("GetScheduledCalculations")]
        public void Run([TimerTrigger("0 */5 * * * *", RunOnStartup = true)] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var jobs = _timeSeriesService.RetrieveScheduledCalculations();



            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
