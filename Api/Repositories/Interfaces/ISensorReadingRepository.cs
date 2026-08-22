namespace Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository seam over the scaffolded <c>Domain.Entities.SensorReading</c> entity, backing
    /// <see cref="Api.Services.TimeSeriesService"/>'s reading-related methods. Kept separate from
    /// <see cref="IScheduledCalcsRepository"/> so each repository is scoped to a single aggregate.
    /// </summary>
    public interface ISensorReadingRepository
    {
        /// <summary>
        /// Gets sensor values for the given sensor ids within the inclusive date range.
        /// </summary>
        Task<List<int>> GetValuesAsync(IEnumerable<int> sensorIds, DateTime dateStart, DateTime dateEnd);

        /// <summary>
        /// Persists a new sensor reading. Returns <c>true</c> if a row was inserted.
        /// </summary>
        Task<bool> AddAsync(int sensorId, int sensorValue, DateTime dateCreated);
    }
}
