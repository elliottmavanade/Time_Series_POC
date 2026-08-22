using Api.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    /// <inheritdoc cref="ISensorCategoryRepository"/>
    public class SensorCategoryRepository : ISensorCategoryRepository
    {
        private readonly TimeSeriesPocDbContext _context;

        public SensorCategoryRepository(TimeSeriesPocDbContext context)
        {
            _context = context;
        }

        public async Task<SensorCategory?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.SensorCategories.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}
