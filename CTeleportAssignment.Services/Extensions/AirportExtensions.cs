using CSharpFunctionalExtensions;
using CTeleportAssignment.Services.Models;
using PvModels = CTeleportAssignment.Providers.Models;

namespace CTeleportAssignment.Services.Extensions
{
    public static class AirportExtensions
    {
        public static Location AsLocation(this Maybe<PvModels.AirportInfo> airportInfo)
        {
            return Maybe<PvModels.Location>
                .From(airportInfo.Value.Location)
                .Map(x => new Location { Lat = x.Lat, Lon = x.Lon }).Value;
        }
    }
}
