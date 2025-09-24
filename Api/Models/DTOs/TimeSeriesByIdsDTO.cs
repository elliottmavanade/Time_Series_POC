namespace Api.Models.DTOs
{
    /// <summary>
    /// Model used to retrieve time series data for multiple sensors by their IDs.
    /// </summary>
    /// <seealso cref="System.IEquatable&lt;Api.Models.DTOs.TimeSeriesByIdsDTO&gt;" />
    /// TODO Edit XML Comment Template for TimeSeriesByIdsDTO
    public record TimeSeriesByIdsDTO
    {
        /// <summary>
        /// Gets the child ids.
        /// </summary>
        /// <value>
        /// The child ids.
        /// </value>
        /// TODO Edit XML Comment Template for ChildIds
        public List<int> ChildIds { get; init; }

        /// <summary>
        /// Gets the timespan.
        /// </summary>
        /// <value>
        /// The timespan.
        /// </value>
        /// TODO Edit XML Comment Template for Timespan
        public int Timespan { get; init; }  
    }
}
