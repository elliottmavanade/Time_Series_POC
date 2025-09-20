namespace Api.Models.DTOs
{
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
