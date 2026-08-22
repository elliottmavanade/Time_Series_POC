using Domain.Entities;

namespace Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository seam over the scaffolded <see cref="SensorCategory"/> entity, present for schema
    /// completeness (not yet consumed by any service).
    /// </summary>
    public interface ISensorCategoryRepository
    {
        /// <summary>
        /// Gets a sensor category by its identifier, or <c>null</c> if it does not exist or the lookup fails.
        /// </summary>
        Task<SensorCategory?> GetByIdAsync(int id);
    }
}
