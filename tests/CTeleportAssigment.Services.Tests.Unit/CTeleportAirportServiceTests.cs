using CTeleportAssigment.Domain;
using CTeleportAssignment.Providers.Exceptions;
using CTeleportAssignment.Providers;
using CTeleportAssignment.Services.Services.Implementations;
using ProviderAirportInfo = CTeleportAssignment.Providers.Models.AirportInfo;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using CTeleportAssignment.Tests.Data;
using NSubstitute.ExceptionExtensions;

namespace CTeleportAssigment.Services.Tests.Unit;

public class CTeleportAirportServiceTests
{
    private readonly IAirportProvider _airportProviderMock;
    private readonly ILogger<CTeleportAirportService> _loggerMock;
    private readonly CTeleportAirportService _service;

    public CTeleportAirportServiceTests()
    {
        _airportProviderMock = Substitute.For<IAirportProvider>();
        _loggerMock = Substitute.For<ILogger<CTeleportAirportService>>();
        _service = new CTeleportAirportService(_airportProviderMock, _loggerMock);
    }

    [Fact]
    public async Task CalculateDistanceAsync_WhenBothAirportsExist_ShouldReturnDistance()
    {
        // Arrange
        var firstAirportInfo = AirportInfoData.AirportInfo();
        var secondAirportInfo = AirportInfoData.AirportInfo();

        var firstIata = new Iata(firstAirportInfo.Iata);
        var secondIata = new Iata(secondAirportInfo.Iata);

        _airportProviderMock.GetAirportInfoByIataAsync(firstIata.Value).Returns(firstAirportInfo);
        _airportProviderMock.GetAirportInfoByIataAsync(secondIata.Value).Returns(secondAirportInfo);

        Location firstAirportLocation = Location.Create(firstAirportInfo.Location.Lat, firstAirportInfo.Location.Lon);
        Location secondAirportLocation = Location.Create(secondAirportInfo.Location.Lat, secondAirportInfo.Location.Lon);

        double expectedDistance = firstAirportLocation.CalculateDistanceInMiles(secondAirportLocation);
        // Act
        var result = await _service.CalculateDistanceAsync(firstIata, secondIata, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.FirstIata.Should().Be(firstIata.Value);
        result.Value.SecondIata.Should().Be(secondIata.Value);
        result.Value.Distance.Value.Should().Be(expectedDistance);

    }

    [Fact]
    public async Task CalculateDistanceAsync_WhenFirstAirportNotFound_ShouldReturnNotFoundError()
    {
        // Arrange
        var firstIata = new Iata(AirportInfoData.AirportInfo().Iata);
        var secondIata = new Iata(AirportInfoData.AirportInfo().Iata);

        _airportProviderMock.GetAirportInfoByIataAsync(firstIata.Value).Returns((ProviderAirportInfo?)null);

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
        var firstIata = new Iata(AirportInfoData.FirstIata);
        var secondIata = new Iata(AirportInfoData.SecondIata);

        var firstAirportInfo = AirportInfoData.AirportInfo();

        _airportProviderMock.GetAirportInfoByIataAsync(firstIata.Value).Returns(firstAirportInfo);
        _airportProviderMock.GetAirportInfoByIataAsync(secondIata.Value).Returns((ProviderAirportInfo?)null);

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
        var firstIata = new Iata(AirportInfoData.FirstIata);
        var secondIata = new Iata(AirportInfoData.SecondIata);

        _airportProviderMock.GetAirportInfoByIataAsync(firstIata.Value).Throws(new InvalidCodeException(firstIata.Value));

        // Act
        var result = await _service.CalculateDistanceAsync(firstIata, secondIata, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<DomainError>();
        result.Error.ErrorType.Should().Be(ErrorType.BadRequest);
    }

    [Fact]
    public async Task CalculateDistanceAsync_WhenUnexpectedExceptionOccurs_ShouldReturnUnexpectedError()
    {
        // Arrange
        var firstIata = new Iata(AirportInfoData.FirstIata);
        var secondIata = new Iata(AirportInfoData.SecondIata);

        _airportProviderMock.GetAirportInfoByIataAsync(Arg.Any<string>())
            .Throws(new ProviderException("Unexpected error"));

        // Act
        var result = await _service.CalculateDistanceAsync(firstIata, secondIata, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().BeOfType<DomainError>();
        result.Error.ErrorType.Should().Be(ErrorType.Unexpected);
        result.Error.ErrorMessage.Should().Contain("Unexpected error");
    }
}
