using Domain.Entities;

namespace Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository seam over the scaffolded <see cref="Sensor"/> entity, backing
    /// <see cref="Api.Services.SensorService.GetSensorAsync"/>.
    /// </summary>
    public interface ISensorRepository
    {
        /// <summary>
        /// Gets a sensor by its identifier, or <c>null</c> if it does not exist or the lookup fails.
        /// </summary>
        Task<Sensor?> GetByIdAsync(int sensorId);
    }
}
