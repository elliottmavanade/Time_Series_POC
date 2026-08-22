using Api.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    /// <inheritdoc cref="ISensorReadingRepository"/>
    public class SensorReadingRepository : ISensorReadingRepository
    {
        private readonly TimeSeriesPocDbContext _context;

        public SensorReadingRepository(TimeSeriesPocDbContext context)
        {
            _context = context;
        }

        public async Task<List<int>> GetValuesAsync(IEnumerable<int> sensorIds, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                var ids = sensorIds as ICollection<int> ?? sensorIds.ToList();

                return await _context.SensorReadings
                    .AsNoTracking()
                    .Where(r => ids.Contains(r.SensorId) && r.DateCreated >= dateStart && r.DateCreated <= dateEnd)
                    .Select(r => r.SensorValue)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<int>();
            }
        }

        public async Task<bool> AddAsync(int sensorId, int sensorValue, DateTime dateCreated)
        {
            try
            {
                _context.SensorReadings.Add(new SensorReading
                {
                    SensorId = sensorId,
                    SensorValue = sensorValue,
                    DateCreated = dateCreated
                });

                var rowsAffected = await _context.SaveChangesAsync();
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}
