using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSeriesDataProcessor.Models
{
    /// <summary>
    /// A record to model the Scheduled_Calcs table in the database.
    /// </summary>
    /// <seealso cref="System.IEquatable&lt;TimeSeriesDataProcessor.Models.SchedueledRuns&gt;" />
    public record ScheduledCalcs
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; init; }

        /// <summary>
        /// Gets the sensor identifier.
        /// </summary>
        /// <value>
        /// The sensor identifier.
        /// </value>
        public int Sensor_Id { get; init; }

    }
}
