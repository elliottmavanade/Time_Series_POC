using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsumerFA.Services.Interfaces
{
    public interface IApiService
    {
        Task<Dictionary<int, List<int>>> GetSensorRelationships(int parentId);
    }
}
