using Domain.Models;

namespace Api.Services.Interfaces
{
    public interface IDashboardService
    {
        Task<List<ScheduledCalcs>> GetScheduledCalcsAsync();

        Task<Dictionary<int, List<int>>> GetSensorRelationshipsAsync(int parentId);
    }
}
