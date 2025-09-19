using Api.Infrastructure;
using Api.Services.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using ProducerFA.Infrastructure;

namespace Api.Controllers
{
    [ApiController]
    [Route(ApiRoutes.Dashboard)]
    public class DashboardController : ControllerBase
    {
        private readonly ILogger<DashboardController> _logger;
        private readonly IDashboardService _dashboardService;

        public DashboardController(ILogger<DashboardController> logger, IDashboardService dashboardService)
        {
            _logger = logger;
            _dashboardService = dashboardService;
        }

        [HttpGet]
        [Route(ActionRoutes.GetScheduledRuns)]
        [ProducesResponseType(typeof(List<ScheduledCalcs>), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetScheduledCalculations()
        {
            var response = await _dashboardService.GetScheduledCalcsAsync();

            if (response == null)
            {
                return NoContent();
            }
            return Ok(response);
        }
    }
}
