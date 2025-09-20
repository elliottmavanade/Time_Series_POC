using Domain.Models;

namespace Api.Services.Interfaces
{
    public interface ISensorService
    {
        Task<Dictionary<int, List<int>>> GetSensorRelationshipsAsync(int parentId);

        Task<Sensor> GetSensorAsync(int parentId);
    }
}
