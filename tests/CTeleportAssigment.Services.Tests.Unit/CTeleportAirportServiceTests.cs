using CTeleportAssigment.Domain;
using CTeleportAssignment.Providers;
using CTeleportAssignment.Services.Models;
using CTeleportAssignment.Services.Services.Contracts;
using CTeleportAssignment.Services.Services.Implementations;
using ProviderAirportInfo = CTeleportAssignment.Providers.Models.AirportInfo;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using CTeleportAssignment.Providers.Exceptions;
namespace CTeleportAssigment.Services.Tests.Unit
{
    public class CTeleportAirportServiceTests
    {
        private readonly Mock<IAirportProvider> _airportProviderMock;
        private readonly Mock<IGeolocationService> _geolocationServiceMock;
        private readonly Mock<ILogger<CTeleportAirportService>> _loggerMock;
        private readonly CTeleportAirportService _service;

        public CTeleportAirportServiceTests()
        {
            _airportProviderMock = new Mock<IAirportProvider>();
            _geolocationServiceMock = new Mock<IGeolocationService>();
            _loggerMock = new Mock<ILogger<CTeleportAirportService>>();

            _service = new CTeleportAirportService(
                _airportProviderMock.Object,
                _geolocationServiceMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task CalculateDistanceAsync_WhenBothAirportsExist_ShouldReturnDistance()
        {
            // Arrange
            var firstIata = new Iata("JFK");
            var secondIata = new Iata("LAX");

            var firstAirportInfo = new ProviderAirportInfo { Iata = "JFK", Name = "John F. Kennedy International Airport" };
            var secondAirportInfo = new ProviderAirportInfo { Iata = "LAX", Name = "Los Angeles International Airport" };

            _airportProviderMock.Setup(p => p.GetAirportInfoByIataAsync(firstIata.Value))
                .ReturnsAsync(firstAirportInfo);

            _airportProviderMock.Setup(p => p.GetAirportInfoByIataAsync(secondIata.Value))
                .ReturnsAsync(secondAirportInfo);

            _geolocationServiceMock.Setup(g => g.CalculateDistance(It.IsAny<Location>(), It.IsAny<Location>()))
                .Returns(3000);

            // Act
            var result = await _service.CalculateDistanceAsync(firstIata, secondIata, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Distance.Should().Be("3000");
            result.Value.UnitType.Should().Be(UnitTypes.MILES);
            result.Value.FirstAirport.Should().Be("JFK");
            result.Value.SecondAirport.Should().Be("LAX");

            _loggerMock.Verify(logger =>
                logger.LogInformation(
                    "The distance between {firstAirport} to {secondAirport} is {distance}",
            "JFK", "LAX", 3000),
                Times.Once);
        }

        [Fact]
        public async Task CalculateDistanceAsync_WhenFirstAirportNotFound_ShouldReturnNotFoundError()
        {
            // Arrange
            var firstIata = new Iata("JFK");
            var secondIata = new Iata("LAX");

            _airportProviderMock.Setup(p => p.GetAirportInfoByIataAsync(firstIata.Value))
                .ReturnsAsync((ProviderAirportInfo?)null);

            // Act
            var result = await _service.CalculateDistanceAsync(firstIata, secondIata, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().BeOfType<DomainError>();
            result.Error.ErrorType.Should().Be(ErrorType.NotFound);
        }

        [Fact]
        public async Task CalculateDistanceAsync_WhenSecondAirportNotFound_ShouldReturnNotFoundError()
        {
            // Arrange
            var firstIata = new Iata("JFK");
            var secondIata = new Iata("LAX");

            var firstAirportInfo = new ProviderAirportInfo { Iata = "JFK", Name = "John F. Kennedy International Airport" };

            _airportProviderMock.Setup(p => p.GetAirportInfoByIataAsync(firstIata.Value))
                .ReturnsAsync(firstAirportInfo);

            _airportProviderMock.Setup(p => p.GetAirportInfoByIataAsync(secondIata.Value))
                .ReturnsAsync((ProviderAirportInfo?)null);

            // Act
            var result = await _service.CalculateDistanceAsync(firstIata, secondIata, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().BeOfType<DomainError>();
            result.Error.ErrorType.Should().Be(ErrorType.NotFound);
        }

        [Fact]
        public async Task CalculateDistanceAsync_WhenProviderThrowsInvalidCodeException_ShouldReturnBadRequestError()
        {
            // Arrange
            var firstIata = new Iata("INVALID");
            var secondIata = new Iata("LAX");

            _airportProviderMock.Setup(p => p.GetAirportInfoByIataAsync(firstIata.Value))
                .ThrowsAsync(new InvalidCodeException(firstIata.Value));

            // Act
            var result = await _service.CalculateDistanceAsync(firstIata, secondIata, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().BeOfType<DomainError>();
            result.Error.ErrorType.Should().Be(ErrorType.BadRequest);
            result.Error.ErrorMessage.Should().Contain("INVALID");
        }

        [Fact]
        public async Task CalculateDistanceAsync_WhenUnexpectedExceptionOccurs_ShouldReturnUnexpectedError()
        {
            // Arrange
            var firstIata = new Iata("JFK");
            var secondIata = new Iata("LAX");

            _airportProviderMock.Setup(p => p.GetAirportInfoByIataAsync(firstIata.Value))
                .ThrowsAsync(new ProviderException("Unexpected error"));

            // Act
            var result = await _service.CalculateDistanceAsync(firstIata, secondIata, CancellationToken.None);

            // Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().BeOfType<DomainError>();
            result.Error.ErrorType.Should().Be(ErrorType.Unexpected);
            result.Error.ErrorMessage.Should().Contain("Unexpected error");
        }
    }

}
