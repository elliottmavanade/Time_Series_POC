using Api.Infrastructure;
using Api.Services.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route(ApiRoutes.TimeSeries)]
    public class TimeSeriesController : ControllerBase
    {
        private readonly ILogger<TimeSeriesController> _logger;
        private readonly ITimeSeriesService _timeSeriesService;

        public TimeSeriesController(ILogger<TimeSeriesController> logger, ITimeSeriesService timeSeriesService)
        {
            _logger = logger;
            _timeSeriesService = timeSeriesService;
        }

        [HttpGet]
        [Route(ActionRoutes.GetScheduledTasks)]
        [ProducesResponseType(typeof(List<ScheduledCalcs>), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetScheduledCalculations()
        {
            var response = await _timeSeriesService.GetScheduledCalcsAsync();

            if (response == null)
            {
                return NoContent();
            }
            return Ok(response);
        }
    }
}
