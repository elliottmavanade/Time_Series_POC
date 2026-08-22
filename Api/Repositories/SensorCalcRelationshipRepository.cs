using Api.Repositories.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Repositories
{
    /// <inheritdoc cref="ISensorCalcRelationshipRepository"/>
    public class SensorCalcRelationshipRepository : ISensorCalcRelationshipRepository
    {
        private readonly TimeSeriesPocDbContext _context;

        public SensorCalcRelationshipRepository(TimeSeriesPocDbContext context)
        {
            _context = context;
        }

        public async Task<List<int>> GetChildSensorIdsAsync(int parentId)
        {
            try
            {
                return await _context.SensorCalcRelationships
                    .AsNoTracking()
                    .Where(r => r.ParentSensorId == parentId)
                    .Select(r => r.ChildSensorId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<int>();
            }
        }
    }
}
