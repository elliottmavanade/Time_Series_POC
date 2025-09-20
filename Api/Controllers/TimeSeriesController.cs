using Api.Infrastructure;
using Api.Models.DTOs;
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
        [Route(ActionRoutes.GetScheduledCalculations)]
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

        [HttpGet]
        [Route(ActionRoutes.GetTimeSeriesDataById)]
        [ProducesResponseType(typeof(List<int>), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTimeSeriesDataById([FromQuery] int id, [FromQuery] int timespan)
        {
            var response = await _timeSeriesService.GetTimeSeriesDataAsync(id, timespan);

            if (response == null)
            {
                return NoContent();
            }
            return Ok(response);
        }

        [HttpPost]
        [Route(ActionRoutes.GetTimeSeriesDataByIds)]
        [ProducesResponseType(typeof(List<int>), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetTimeSeriesDataByIds(TimeSeriesByIdsDTO request)
        {
            var response = new List<int>();

            response = await _timeSeriesService.GetTimeSeriesDataAsync(request.ChildIds.GetEnumerator(), request.Timespan);
         

            if (response.Count == 0)
            {
                return NoContent();
            }
            return Ok(response);
        }
    }
}
