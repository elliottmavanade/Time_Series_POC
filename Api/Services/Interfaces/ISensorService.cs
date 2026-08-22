namespace Api.Services.Interfaces
{
    public interface ISensorService
    {
        Task<Tuple<int, List<int>>> GetSensorRelationshipsAsync(int parentId);

        Task<Domain.Entities.Sensor?> GetSensorAsync(int sensorId);
    }
}
