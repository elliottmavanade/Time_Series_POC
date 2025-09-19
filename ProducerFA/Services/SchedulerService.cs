using Microsoft.Extensions.Logging;
using ProducerFA.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeSeriesDataProcessor.Models;
using TimeSeriesDataProcessor.Repository;

namespace ProducerFA.Services
{
    public class SchedulerService : ISchedulerService
    {
        private readonly ILogger<SchedulerService> _logger;
        private readonly DatabaseConnection _databaseConnection;

        public SchedulerService(ILogger<SchedulerService> logger, DatabaseConnection databaseConnection)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _databaseConnection = databaseConnection ?? throw new ArgumentNullException(nameof(databaseConnection));
        }

        public async Task<List<ScheduledCalcs>> RetrieveScheduledCalculations()
        {
            var jobs = await _databaseConnection.GetScheduledCalcsAsync();

            return jobs;
        }
    }
}
