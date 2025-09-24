namespace Api.Models.DTOs
{
    /// <summary>
    /// Model to retrieve data out of the Sensor table.
    /// SensorMetadata class to transfer metadata as a JSON string.
    /// </summary>
    /// <seealso cref="System.IEquatable&lt;Api.Models.DTOs.SensorDTO&gt;" />
    /// TODO Edit XML Comment Template for SensorDTO
    public record SensorDTO
    {
        public required int Id { get; init; }
        public required int Sensor_Model_Id { get; init; }
        public required string Location { get; init; }
        public string? Metadata { get; init; }
    }

    internal record SensorMetadata
    {
        public string? Aggregation { get; init; }
        public int? Timespan { get; init; }
    }
}
