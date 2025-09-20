namespace Api.Models.DTOs
{
    /// <summary>
    /// DTO to add a time series result.
    /// </summary>
    /// <seealso cref="System.IEquatable&lt;Api.Models.DTOs.TimeSeriesResultDTO&gt;" />
    public record TimeSeriesResultDTO
    {
        /// <summary>
        /// Gets the sensor identifier.
        /// </summary>
        /// <value>
        /// The sensor identifier.
        /// </value>
        public int SensorId { get; init; }
        /// <summary>
        /// Gets the sensor value.
        /// </summary>
        /// <value>
        /// The sensor value.
        /// </value>
        public int SensorValue { get; init; }  
    }
}
