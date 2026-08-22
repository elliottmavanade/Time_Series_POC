using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services
{
    public class SensorService : ISensorService
    {
        private readonly ISensorRepository _sensorRepository;
        private readonly ISensorCalcRelationshipRepository _sensorCalcRelationshipRepository;

        public SensorService(ISensorRepository sensorRepository, ISensorCalcRelationshipRepository sensorCalcRelationshipRepository)
        {
            _sensorRepository = sensorRepository;
            _sensorCalcRelationshipRepository = sensorCalcRelationshipRepository;
        }

        public async Task<Tuple<int, List<int>>> GetSensorRelationshipsAsync(int parentId)
        {
            var childIds = await _sensorCalcRelationshipRepository.GetChildSensorIdsAsync(parentId);
            return new Tuple<int, List<int>>(parentId, childIds);
        }

        public async Task<Domain.Entities.Sensor?> GetSensorAsync(int sensorId)
        {
            return await _sensorRepository.GetByIdAsync(sensorId);
        }
    }
}
