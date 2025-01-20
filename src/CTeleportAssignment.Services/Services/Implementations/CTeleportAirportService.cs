using CSharpFunctionalExtensions;
using CTeleportAssigment.Domain;
using CTeleportAssignment.Providers;
using CTeleportAssignment.Providers.Exceptions;
using CTeleportAssignment.Services.Models;
using CTeleportAssignment.Services.Services.Contracts;
using Microsoft.Extensions.Logging;

namespace CTeleportAssignment.Services.Services.Implementations
{
    public class CTeleportAirportService(IAirportProvider airportProvider
           , ILogger<CTeleportAirportService> logger) : IAirportService
    {
        private readonly IAirportProvider _airportProvider = airportProvider;
        private readonly ILogger<CTeleportAirportService> _logger = logger;

        public async Task<Result<AirportPair, DomainError>> CalculateDistanceAsync(Iata firstIata, Iata secondIata, CancellationToken cancellationToken)
        {
            try
            {
                var firstAirportInfo = await _airportProvider.GetAirportInfoByIataAsync(firstIata.Value);

                if (firstAirportInfo is null)
                {
                    return Result.Failure<AirportPair, DomainError>(DomainError.NotFound());
                }

                var secondAirportInfo = await _airportProvider.GetAirportInfoByIataAsync(secondIata.Value);

                if (secondAirportInfo is null)
                {
                    return Result.Failure<AirportPair, DomainError>(DomainError.NotFound());
                }

                Location firstAirportLocation = Location.Create(firstAirportInfo.Location.Lat, firstAirportInfo.Location.Lon);
                Location secondAirportLocation = Location.Create(secondAirportInfo.Location.Lat, secondAirportInfo.Location.Lon);

                var distance = firstAirportLocation.CalculateDistanceInMiles(secondAirportLocation);

                _logger.LogInformation("The distance between {firstAirport} to {secondAirport} is {distance}", firstAirportInfo.Iata, secondAirportInfo.Iata, distance);

                var airport = new AirportPair(firstAirportInfo.Iata, secondAirportInfo.Iata, Distance.Mile(Convert.ToDouble(distance)));

                return Result.Success<AirportPair, DomainError>(airport);
            }
            catch (InvalidCodeException exp)
            {
                _logger.LogError(message: exp.Message);
                return Result.Failure<AirportPair, DomainError>(DomainError.BadRequest(exp.Message));
            }
            catch (NotFoundException exp)
            {
                _logger.LogError(message: exp.Message);
                return Result.Failure<AirportPair, DomainError>(DomainError.NotFound(exp.Message));
            }
            catch (ProviderException exp)
            {
                _logger.LogError(message: exp.Message);
                return Result.Failure<AirportPair, DomainError>(DomainError.UnExpected(exp.Message));
            }
        }
    }
}
