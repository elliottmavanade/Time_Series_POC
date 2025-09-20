namespace Api.Models.DTOs
{
    public record SensorDTO
    {
        public int Id { get; init; }
        public int Sensor_Model_Id { get; init; }
        public string Location { get; init; }
        public string? Metadata { get; init; }
    }

    internal record SensorMetadata
    {
        public string? Aggregation { get; init; }
        public int? Timespan { get; init; }
    }
}
