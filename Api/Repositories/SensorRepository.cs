using Api.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    /// <inheritdoc cref="ISensorRepository"/>
    public class SensorRepository : ISensorRepository
    {
        private readonly TimeSeriesPocDbContext _context;

        public SensorRepository(TimeSeriesPocDbContext context)
        {
            _context = context;
        }

        public async Task<Sensor?> GetByIdAsync(int sensorId)
        {
            try
            {
                return await _context.Sensors.AsNoTracking().FirstOrDefaultAsync(s => s.Id == sensorId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}
