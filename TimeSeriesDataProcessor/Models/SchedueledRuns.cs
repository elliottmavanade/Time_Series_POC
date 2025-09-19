using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeSeriesDataProcessor.Models
{
    public record SchedueledRuns
    {
        public string Name { get; init; }

        public int Sensor_Id { get; init; }

    }
}
