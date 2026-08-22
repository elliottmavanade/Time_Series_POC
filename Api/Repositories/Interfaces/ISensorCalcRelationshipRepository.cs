namespace Api.Repositories.Interfaces
{
    /// <summary>
    /// Repository seam over the self-referencing <c>Sensor_Calc_Relationships</c> join table (modeled as
    /// an explicit <c>Domain.Entities.SensorCalcRelationship</c> entity, not an EF skip-navigation
    /// many-to-many), backing <see cref="Api.Services.SensorService.GetSensorRelationshipsAsync"/>.
    /// </summary>
    public interface ISensorCalcRelationshipRepository
    {
        /// <summary>
        /// Gets the child sensor ids configured for the given parent sensor id.
        /// </summary>
        Task<List<int>> GetChildSensorIdsAsync(int parentId);
    }
}
