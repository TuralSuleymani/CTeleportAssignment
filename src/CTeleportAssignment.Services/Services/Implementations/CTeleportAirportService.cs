using CSharpFunctionalExtensions;
using CTeleportAssigment.Domain;
using CTeleportAssignment.Providers;
using CTeleportAssignment.Providers.Exceptions;
using CTeleportAssignment.Services.Extensions;
using CTeleportAssignment.Services.Models;
using CTeleportAssignment.Services.Services.Contracts;
using Microsoft.Extensions.Logging;

namespace CTeleportAssignment.Services.Services.Implementations
{
    public class CTeleportAirportService(IAirportProvider airportProvider
           , IGeolocationService geolocationService
           , ILogger<CTeleportAirportService> logger) : IAirportService
    {
        private readonly IAirportProvider _airportProvider = airportProvider;
        private readonly IGeolocationService _geolocationService = geolocationService;
        private readonly ILogger<CTeleportAirportService> _logger = logger;

        public async Task<Result<AirportInfo, DomainError>> CalculateDistanceAsync(Iata firstIata, Iata secondIata, CancellationToken cancellationToken)
        {
            try
            {
                var firstAirportInfo = await _airportProvider.GetAirportInfoByIataAsync(firstIata.Value);
                
                if (firstAirportInfo is null)
                {
                    return Result.Failure<AirportInfo, DomainError>(DomainError.NotFound());
                }

                var secondAirportInfo = await _airportProvider.GetAirportInfoByIataAsync(firstIata.Value);

                if (secondAirportInfo is null)
                {
                    return Result.Failure<AirportInfo, DomainError>(DomainError.NotFound());
                }

                var distance = _geolocationService
                                .CalculateDistance(firstAirportInfo.ServiceLocation(), secondAirportInfo.ServiceLocation())
                                .AsMiles();

                _logger.LogInformation("The distance between {firstAirport} to {secondAirport} is {distance}", firstAirportInfo.Iata, secondAirportInfo.Iata, distance);

                return new AirportInfo
                {
                    FirstAirport = firstAirportInfo.Iata,
                    SecondAirport = secondAirportInfo.Iata,
                    Distance = distance,
                    UnitType = UnitTypes.MILES
                };
            }
            catch (InvalidCodeException exp)
            {
                _logger.LogError(message: exp.Message);
                return Result.Failure<AirportInfo, DomainError>(DomainError.BadRequest(exp.Message));
            }
            catch (NotFoundException exp)
            {
                _logger.LogError(message: exp.Message);
                return Result.Failure<AirportInfo, DomainError>(DomainError.NotFound(exp.Message));
            }
            catch (ProviderException exp)
            {
                _logger.LogError(message: exp.Message);
                return Result.Failure<AirportInfo, DomainError>(DomainError.UnExpected(exp.Message));
            }
        }
    }
}
