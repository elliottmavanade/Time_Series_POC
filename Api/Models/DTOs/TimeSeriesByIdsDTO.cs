namespace Api.Models.DTOs
{
    public record TimeSeriesByIdsDTO
    {
        public List<int> ChildIds { get; init; }

        public int Timespan { get; init; }  
    }
}
