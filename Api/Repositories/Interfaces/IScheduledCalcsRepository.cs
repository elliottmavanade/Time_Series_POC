using Domain.Entities;

namespace Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository seam over the scaffolded <see cref="ScheduledCalc"/> entity, backing
    /// <see cref="Api.Services.TimeSeriesService.GetScheduledCalcsAsync"/>.
    /// </summary>
    public interface IScheduledCalcsRepository
    {
        /// <summary>
        /// Gets every scheduled calculation.
        /// </summary>
        Task<List<ScheduledCalc>> GetAllAsync();
    }
}
