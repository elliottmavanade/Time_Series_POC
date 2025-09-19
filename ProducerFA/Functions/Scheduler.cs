using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ProducerFA.Services.Interfaces;

namespace ProducerFA.Functions
{
    public class Scheduler
    {
        private readonly ILogger _logger;
        private readonly ISchedulerService _schedulerService;
        public Scheduler(ILoggerFactory loggerFactory, ISchedulerService schedulerService, I )
        {
            _logger = loggerFactory.CreateLogger<Scheduler>();
            _schedulerService = schedulerService;
        }

        [Function("GetScheduledCalculations")]
        public void Run([TimerTrigger("0 0 0 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            var jobs = _schedulerService.RetrieveScheduledCalculations();



            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
