using CSharpFunctionalExtensions;
using CTeleportAssignment.Providers;
using CTeleportAssignment.Providers.Exceptions;
using CTeleportAssignment.Services.Exceptions;
using CTeleportAssignment.Services.Extensions;
using CTeleportAssignment.Services.Models;
using CTeleportAssignment.Services.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CTeleportAssignment.Services.Commands
{
    public class GetDistanceBetweenAirportsQueryHandler : IRequestHandler<GetDistanceBetweenAirportsQuery, Maybe<AirportInfo>>
    {
        private const int STRICT_IATA_LENGTH = 3;
        private const string IATA_CODE_LENGTH_ERROR = "IATA Code should be 3 letter length";
        private const string SERVICE_FAILED_ERROR = "Service failed to retrieve Airport information";
        private readonly IAirportProvider _airportProvider;
        private readonly IGeolocationService _geolocationService;
        private readonly ILogger<GetDistanceBetweenAirportsQueryHandler> _logger;

        public GetDistanceBetweenAirportsQueryHandler(IAirportProvider airportProvider
            , IGeolocationService geolocationService
            , ILogger<GetDistanceBetweenAirportsQueryHandler> logger)
        {
            _airportProvider = airportProvider;
            _geolocationService = geolocationService;
            _logger = logger;
        }

        public async Task<Maybe<AirportInfo>> Handle(GetDistanceBetweenAirportsQuery request, CancellationToken cancellationToken)
        {
            if (request.firstAirportIata.Length != STRICT_IATA_LENGTH || request.secondAirportIata.Length != STRICT_IATA_LENGTH)
            {
                _logger.LogError(IATA_CODE_LENGTH_ERROR);
                throw new InvalidIataProvidedException(IATA_CODE_LENGTH_ERROR);
            }

            try
            {
                var firstAirportInfo = await _airportProvider.GetAirportInfoByIataAsync(request.firstAirportIata);
                var secondAirportInfo = await _airportProvider.GetAirportInfoByIataAsync(request.secondAirportIata);

                if (firstAirportInfo.HasNoValue || secondAirportInfo.HasNoValue)
                {
                    _logger.LogError(SERVICE_FAILED_ERROR);
                    throw new ServiceException(SERVICE_FAILED_ERROR);
                }

                var distance = _geolocationService
                                .CalculateDistance(firstAirportInfo.AsLocation(), secondAirportInfo.AsLocation())
                                .AsMiles();

                _logger.LogInformation("The distance between {firstAirport} to {secondAirport} is {distance}", firstAirportInfo.Value.Iata, secondAirportInfo.Value.Iata, distance);

                //use automapper to map it
                return new AirportInfo
                {
                    FirstAirport = firstAirportInfo.Value.Iata,
                    SecondAirport = secondAirportInfo.Value.Iata,
                    Distance = distance,
                    UnitType = UnitTypes.MILES
                };
            }
            catch (InvalidCodeException exp)
            {
                _logger.LogError(exp.Message);
                throw new InvalidIataProvidedException(exp.Message);
            }
            catch (NotFoundException exp)
            {
                _logger.LogError(exp.Message);
                throw new IataNotFoundException(exp.Message);
            }
            catch (ProviderException exp)
            {
                _logger.LogError(exp.Message);
                throw new ServiceException(exp.Message);
            }
        }
    }
}
