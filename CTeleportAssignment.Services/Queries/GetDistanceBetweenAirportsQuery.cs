using CSharpFunctionalExtensions;
using CTeleportAssignment.Services.Models;
using MediatR;

namespace CTeleportAssignment.Services.Queries
{
    public sealed record GetDistanceBetweenAirportsQuery(string firstAirportIata, string secondAirportIata) : IRequest<Maybe<AirportInfo>>;
   
}
