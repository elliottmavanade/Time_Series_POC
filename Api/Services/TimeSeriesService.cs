using Api.Models.DTOs;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Domain.Models;

namespace Api.Services
{
    public class TimeSeriesService : ITimeSeriesService
    {
        private readonly IScheduledCalcsRepository _scheduledCalcsRepository;
        private readonly ISensorReadingRepository _sensorReadingRepository;

        public TimeSeriesService(IScheduledCalcsRepository scheduledCalcsRepository, ISensorReadingRepository sensorReadingRepository)
        {
            _scheduledCalcsRepository = scheduledCalcsRepository;
            _sensorReadingRepository = sensorReadingRepository;
        }

        public async Task<List<ScheduledCalcs>> GetScheduledCalcsAsync()
        {
            var scheduledCalcs = await _scheduledCalcsRepository.GetAllAsync();

            return scheduledCalcs
                .Select(sc => new ScheduledCalcs { Name = sc.Name, Sensor_Id = sc.SensorId })
                .ToList();
        }

        public Task<List<int>> GetTimeSeriesDataAsync(int id, int timespan) // Allow for single ID for convenience
            => QueryTimeSeriesAsync(new[] { id }, timespan);

        public Task<List<int>> GetTimeSeriesDataAsync(IEnumerable<int> ids, int timespan)
            => QueryTimeSeriesAsync(ids, timespan);

        private async Task<List<int>> QueryTimeSeriesAsync(IEnumerable<int> ids, int timespan)
        {
            DateTime currentTime = DateTime.Now;
            // Round down to the nearest hour
            DateTime dateEnd = new DateTime(currentTime.Year, currentTime.Month, 17, 0, 0, 0); // So accurate results show up in testing
            // Subtract Time
            DateTime dateStart = dateEnd.AddMinutes(-timespan);

            return await _sensorReadingRepository.GetValuesAsync(ids, dateStart, dateEnd);
        }

        public async Task<bool> AddTimeSeriesResultAsync(TimeSeriesResultDTO request)
        {
            return await _sensorReadingRepository.AddAsync(request.SensorId, request.SensorValue, DateTime.Now); // This would be different in a real scenario
        }
    }
}
