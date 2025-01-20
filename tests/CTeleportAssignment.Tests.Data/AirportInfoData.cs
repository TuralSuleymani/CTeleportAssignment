using Bogus;
using CTeleportAssignment.Providers.Models;

namespace CTeleportAssignment.Tests.Data
{
    public static class AirportInfoData
    {
        public static string InvalidIata { get; } = "INVALID";
        public static string FirstIata { get; } = "ABD";
        public static string SecondIata { get; } = "AAA";
        public static AirportInfo AirportInfo()
        {
            var timeZoneRegions = new List<string>
                {
                    "America/New_York",
                    "Europe/London",
                    "Asia/Tokyo",
                    "Australia/Sydney",
                    "America/Los_Angeles",
                    "Europe/Paris",
                    "Asia/Dubai",
                    "Africa/Johannesburg",
                    "America/Chicago",
                    "Asia/Kolkata"
                };

            var locationFaker = new Faker<Location>()
            .RuleFor(l => l.Lat, f => f.Address.Latitude())
            .RuleFor(l => l.Lon, f => f.Address.Longitude());

            // Configure the faker for AirportInfo
            var airportFaker = new Faker<AirportInfo>()
                .RuleFor(a => a.Country, f => f.Address.Country())
                .RuleFor(a => a.CityIata, f => f.Random.AlphaNumeric(3).ToUpper())
                .RuleFor(a => a.Iata, f => f.Random.AlphaNumeric(3).ToUpper())
                .RuleFor(a => a.City, f => f.Address.City())
                .RuleFor(a => a.TimeZoneRegionName, f => f.PickRandom(timeZoneRegions))
                .RuleFor(a => a.CountryIata, f => f.Random.AlphaNumeric(3).ToUpper())
                .RuleFor(a => a.Rating, f => f.Random.Int(1, 5))
                .RuleFor(a => a.Name, f => $"{f.Company.CompanyName()} Airport")
                .RuleFor(a => a.Location, f => locationFaker.Generate())
                .RuleFor(a => a.Type, f => f.PickRandom("International", "Domestic", "Regional"))
                .RuleFor(a => a.Hubs, f => f.Random.Int(1, 10));

            return airportFaker.Generate();
        }
    }
}
