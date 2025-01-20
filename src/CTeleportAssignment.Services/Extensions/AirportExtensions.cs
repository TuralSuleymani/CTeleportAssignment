using CTeleportAssignment.Services.Models;
using PvModels = CTeleportAssignment.Providers.Models;

namespace CTeleportAssignment.Services.Extensions
{
    public static class AirportExtensions
    {
        public static Location ServiceLocation(this PvModels.AirportInfo airportInfo)
        {
            return new Location() {  Lat = airportInfo.Location.Lat, Lon = airportInfo.Location.Lon };
        }
    }
}
