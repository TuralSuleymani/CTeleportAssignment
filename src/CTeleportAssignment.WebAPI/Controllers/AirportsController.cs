using CTeleportAssigment.Domain;
using CTeleportAssignment.Services.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace CTeleportAssignment.WebAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AirportsController : BaseController
    {
        private readonly IAirportService _airportService;

        public AirportsController(IAirportService airportService, ILogger<AirportsController> logger)
            : base(logger)
        {
            _airportService = airportService;
        }

        [HttpGet("distance/{firstIata}/{secondIata}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetDistanceBetweenAirports(string firstIata, string secondIata)
        {
           Iata _firstIata = Iata.Create(firstIata);
            Iata _secondIata = Iata.Create(secondIata);
            var airportServiceResponse = await _airportService.CalculateDistanceAsync(_firstIata, _secondIata, CancellationToken.None);
            if (airportServiceResponse.IsSuccess)
                return Ok(airportServiceResponse.Value);
            else
                return HandleError(airportServiceResponse.Error);
        }

    }
}
