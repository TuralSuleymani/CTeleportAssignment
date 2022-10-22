using CTeleportAssignment.Providers;
using CTeleportAssignment.Services.Commands;
using CTeleportAssignment.Services.Models;
using Moq;
using CTeleportAssignment.Services.Queries;
using CTeleportAssignment.Services.Exceptions;
using CTeleportAssignment.Services.UnitTests.Extensions;
using Microsoft.Extensions.Logging;

namespace CTeleportAssignment.Services.UnitTests
{
    public class AirportServiceTests
    {
        [Theory]
        [InlineData("ABB", "ACC", 3450,"2.14")]
        [InlineData("MFT", "IAM", 7688, "4.78")]
        [InlineData("MTF", "VBB", 13455, "8.36")]
        public async void CalculateDistance_CorrectArgumentsGiven_SuccessfullyCalculates(string firstIataCode, string secondIataCode, int inMeters, string inMiles)
        {
            //Arrange
            Mock<IAirportProvider> provider = new Mock<IAirportProvider>();
               provider.SetupWithDefaults();

            var mockedLogger = new Mock<ILogger<GetDistanceBetweenAirportsQueryHandler>>();

            Mock<IGeolocationService> geoService = new Mock<IGeolocationService>();
            geoService.Setup(x => x.CalculateDistance(It.IsAny<Location>(), It.IsAny<Location>()))
                .Returns(inMeters);


           var handler = new GetDistanceBetweenAirportsQueryHandler(provider.Object, geoService.Object, mockedLogger.Object);

            GetDistanceBetweenAirportsQuery query
                = new GetDistanceBetweenAirportsQuery(firstIataCode, secondIataCode);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.Equal(inMiles, result.Value.Distance);
           
        }

        [Theory]
        [InlineData("IAFV","IAT")]
        [InlineData("FBB","IAFVDFSGFD")]
        [InlineData("ib","MVC")]
        public void CalculateDistance_InvalidLengthArgumentsGiven_ThrowsArgumentException(string firstIataCode, string secondIataCode)
        {
            //Arrange
            Mock<IAirportProvider> provider = new Mock<IAirportProvider>();

            Mock<IGeolocationService> geoService = new Mock<IGeolocationService>();

            var mockedLogger = new Mock<ILogger<GetDistanceBetweenAirportsQueryHandler>>();

            var handler = new GetDistanceBetweenAirportsQueryHandler(provider.Object, geoService.Object,mockedLogger.Object);

            GetDistanceBetweenAirportsQuery query
                = new GetDistanceBetweenAirportsQuery(firstIataCode, secondIataCode);

            //Act && Assert
            _ = Assert.ThrowsAsync<ArgumentException>(async () => await handler.Handle(query, CancellationToken.None));

        }

        [Theory]
        [InlineData("ABB", "ACC")]
        [InlineData("MFT", "IAM")]
        [InlineData("MTF", "VBB")]
        public void CalculateDistance_IncorrectArgumentsGiven_ThrowsInvalidIataProvidedException(string firstIataCode, string secondIataCode)
        {
            //Arrange
            Mock<IAirportProvider> provider = new Mock<IAirportProvider>();

            provider.SetupWithDefaults();

            var mockedLogger = new Mock<ILogger<GetDistanceBetweenAirportsQueryHandler>>();

            Mock<IGeolocationService> geoService = new Mock<IGeolocationService>();

            var response = new GetDistanceBetweenAirportsQueryHandler(provider.Object, geoService.Object,mockedLogger.Object);

            GetDistanceBetweenAirportsQuery query
                = new GetDistanceBetweenAirportsQuery(firstIataCode, secondIataCode);

            //Act && Assert
            _ = Assert.ThrowsAsync<InvalidIataProvidedException>(async () => await response.Handle(query, CancellationToken.None));

        }

    }
}
