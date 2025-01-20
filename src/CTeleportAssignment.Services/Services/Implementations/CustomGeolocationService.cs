using CTeleportAssignment.Services.Models;
using CTeleportAssignment.Services.Services.Contracts;

namespace CTeleportAssignment.Services.Services.Implementations
{
    public class CustomGeolocationService : IGeolocationService
    {
        public double CalculateDistance(Location point1, Location point2)
        {
            var d1 = point1.Lat * (Math.PI / 180.0);
            var num1 = point1.Lon * (Math.PI / 180.0);
            var d2 = point2.Lat * (Math.PI / 180.0);
            var num2 = point2.Lon * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }
    }
}
