namespace ConsumerFA.Models
{
    /// <summary>
    /// Shape of the <c>GetSensor</c> HTTP response now that <c>Api</c> returns the raw scaffolded
    /// <c>Domain.Entities.Sensor</c> entity (with <c>Metadata</c> as an un-parsed JSON string) instead
    /// of the previously hand-computed <c>Domain.Models.Sensor</c> (which had <c>Aggregation</c>/<c>Timespan</c>
    /// already resolved).
    /// </summary>
    internal record ApiSensorResponse
    {
        public int Id { get; init; }
        public int SensorModelId { get; init; }
        public string Location { get; init; } = string.Empty;
        public string? Metadata { get; init; }
    }

    /// <summary>
    /// Shape of the JSON stored in <see cref="ApiSensorResponse.Metadata"/>.
    /// </summary>
    internal record SensorMetadata
    {
        public string? Aggregation { get; init; }
        public int? Timespan { get; init; }
    }
}
