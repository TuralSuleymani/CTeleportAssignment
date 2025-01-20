using CTeleportAssigment.Domain;
using CTeleportAssignment.Services.Services.Contracts;
using CTeleportAssignment.WebAPI.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace CTeleportAssignment.WebAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AirportsController : BaseController
    {
        private readonly IAirportService _airportService;

        protected AirportsController(IAirportService airportService, ILogger<AirportsController> logger)
            : base(logger)
        {
            _airportService = airportService;
        }

        [HttpGet("distance/{firstIata}/{secondIata}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDistanceBetweenAirports(Iata firstIata, Iata secondIata)
        {
            var airportServiceResponse = await _airportService.CalculateDistanceAsync(firstIata, secondIata, CancellationToken.None);
            if (airportServiceResponse.IsSuccess)
                return NoContent();
            else
                return HandleError(airportServiceResponse.Error);
        }

    }
}
