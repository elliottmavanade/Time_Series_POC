using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    /// <summary>
    /// An Enum to represent the types of aggregation 
    /// that can be performed on a set of data.
    /// </summary>
    public enum AggregationType
    {
        /// <summary>
        /// The sum
        /// </summary>
        Sum,
        /// <summary>
        /// The average
        /// </summary>
        Avg
    }
}
