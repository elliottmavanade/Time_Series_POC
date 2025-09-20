using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    /// <summary>
    /// Model representing the Sensor Table in the database.
    /// </summary>
    /// <seealso cref="System.IEquatable&lt;Domain.Models.Sensor&gt;" />
    public record Sensor
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; init; }
        /// <summary>
        /// Gets the sensor model identifier.
        /// </summary>
        /// <value>
        /// The sensor model identifier.
        /// </value>
        public int Sensor_Model_Id { get; init; }
        /// <summary>
        /// Gets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public string Location { get; init; }
        /// <summary>
        /// Gets the aggregation.
        /// </summary>
        /// <value>
        /// The aggregation.
        /// </value>
        public AggregationType? Aggregation { get; init; }
        /// <summary>
        /// Gets the timespan.
        /// </summary>
        /// <value>
        /// The timespan.
        /// </value>
        public int? Timespan { get; init; }
    }
}
