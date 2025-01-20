using CTeleportAssignment.Services.Models;

namespace CTeleportAssignment.Services.Services.Contracts
{
    public interface IGeolocationService
    {
        public double CalculateDistance(Location from, Location to);
    }
}
