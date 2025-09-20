using Api.Infrastructure;
using Api.Services.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route(ApiRoutes.Sensor)]
    public class SensorController : ControllerBase
    {
        private readonly ILogger<SensorController> _logger;
        private readonly ISensorService _sensorService;

        public SensorController(ILogger<SensorController> logger, ISensorService sensorService)
        {
            _logger = logger;
            _sensorService = sensorService;
        }

        [HttpGet]
        [Route(ActionRoutes.GetSensorRelationships)]
        [ProducesResponseType(typeof(List<ScheduledCalcs>), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSensorRelationships(int parentId)
        {
            var response = await _sensorService.GetSensorRelationshipsAsync(parentId);

            if (response == null)
            {
                return NoContent();
            }
            return Ok(response);
        }

        [HttpGet]
        [Route(ActionRoutes.GetSensor)]
        [ProducesResponseType(typeof(Sensor), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSensor(int sensorId)
        {
            var response = await _sensorService.GetSensorAsync(sensorId);

            if (response == null)
            {
                return NoContent();
            }
            return Ok(response);
        }
    }
}
