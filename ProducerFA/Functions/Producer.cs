using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ProducerFA.Services.Interfaces;

namespace ProducerFA.Functions
{
    public class Producer
    {
        private readonly ILogger _logger;
        private readonly ISchedulerService _schedulerService;
        public Producer(ILoggerFactory loggerFactory, ISchedulerService schedulerService)
        {
            _logger = loggerFactory.CreateLogger<Producer>();
            _schedulerService = schedulerService;
        }

        [Function("GetScheduledCalculations")]
        public void Run([TimerTrigger("0 0 0 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            _schedulerService.RetrieveScheduledRuns();

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
