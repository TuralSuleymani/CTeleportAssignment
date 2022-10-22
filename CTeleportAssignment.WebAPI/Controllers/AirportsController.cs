using CTeleportAssignment.Services.Queries;
using CTeleportAssignment.WebAPI.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CTeleportAssignment.WebAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AirportsController : ApiControllerBase
    {
        public AirportsController(IMediator mediator) : base(mediator) { }

        [HttpGet("distance/{firstIata}/{secondIata}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<IActionResult> GetDistanceBetweenAirports(string firstIata, string secondIata) => Send(new GetDistanceBetweenAirportsQuery(firstIata, secondIata));
    }
}
