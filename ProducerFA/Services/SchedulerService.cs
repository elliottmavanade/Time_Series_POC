using ProducerFA.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerFA.Services
{
    public class SchedulerService : ISchedulerService
    {
        public async Task RetrieveScheduledRuns()
        {
            await Task.CompletedTask;
        }
    }
}
