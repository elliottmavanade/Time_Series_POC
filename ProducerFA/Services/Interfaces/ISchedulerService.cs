using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerFA.Services.Interfaces
{
    public interface ISchedulerService
    {
        Task RetrieveScheduledRuns();
    }
}
