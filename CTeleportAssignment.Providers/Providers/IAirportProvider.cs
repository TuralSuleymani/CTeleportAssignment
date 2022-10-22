using CSharpFunctionalExtensions;
using CTeleportAssignment.Providers.Models;

namespace CTeleportAssignment.Providers
{
    public interface IAirportProvider
    {
        Task<Maybe<AirportInfo>> GetAirportInfoByIataAsync(string iata);
    }
}
