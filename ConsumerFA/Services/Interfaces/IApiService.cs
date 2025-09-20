using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumerFA.Services.Interfaces
{
    public interface IApiService
    {
        Task<Tuple<int, List<int>>> GetSensorRelationships(int parentId);

        Task<Sensor> GetSensor(int sensorId);
    }
}
