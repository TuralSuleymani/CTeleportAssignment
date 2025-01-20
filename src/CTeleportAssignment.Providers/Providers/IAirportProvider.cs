using CTeleportAssignment.Providers.Models;

namespace CTeleportAssignment.Providers
{
    public interface IAirportProvider
    {
        Task<AirportInfo?> GetAirportInfoByIataAsync(string iata);
    }
}
