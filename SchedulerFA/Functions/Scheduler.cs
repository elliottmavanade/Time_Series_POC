using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SchedulerFA.Functions
{
    public class Scheduler
    {
        private readonly ILogger _logger;

        public Scheduler(ILoggerFactory loggerFactory, )
        {
            _logger = loggerFactory.CreateLogger<Scheduler>();
        }

        [Function("Function1")]
        public void Run([TimerTrigger("0 0 0 * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            


            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
            }
        }
    }
}
