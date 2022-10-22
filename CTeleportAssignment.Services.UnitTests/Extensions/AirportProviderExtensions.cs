using CSharpFunctionalExtensions;
using CTeleportAssignment.Providers;
using Moq;
using PVModels = CTeleportAssignment.Providers.Models;

namespace CTeleportAssignment.Services.UnitTests.Extensions
{
    public static class AirportProviderExtensions
    {
        public static void SetupWithDefaults(this Mock<IAirportProvider> provider)
        {
            var airportInfo = Maybe<PVModels.AirportInfo>.From(new PVModels.AirportInfo
            {
                City = "Baku",
                Location = new PVModels.Location
                {
                    Lat = 45,
                    Lon = 77
                },
                 Country = "Azerbiajan",
                  Iata = "FZL",
                   Rating = 4
            });
            provider.Setup(x => x.GetAirportInfoByIataAsync(It.IsAny<string>())).ReturnsAsync(airportInfo);
        }
    }
}
