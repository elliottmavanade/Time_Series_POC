using Api.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    /// <inheritdoc cref="IScheduledCalcsRepository"/>
    public class ScheduledCalcsRepository : IScheduledCalcsRepository
    {
        private readonly TimeSeriesPocDbContext _context;

        public ScheduledCalcsRepository(TimeSeriesPocDbContext context)
        {
            _context = context;
        }

        public async Task<List<ScheduledCalc>> GetAllAsync()
        {
            try
            {
                return await _context.ScheduledCalcs.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<ScheduledCalc>();
            }
        }
    }
}
