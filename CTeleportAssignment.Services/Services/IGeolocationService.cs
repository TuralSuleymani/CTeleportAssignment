using CTeleportAssignment.Services.Models;

namespace CTeleportAssignment.Services
{
    public interface IGeolocationService
    {
        public double CalculateDistance(Location point1, Location point2);
    }
}
