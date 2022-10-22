using CTeleportAssignment.Providers.Config;
using CTeleportAssignment.Providers.Exceptions;
using CTeleportAssignment.Providers.UnitTests.Factories;

namespace CTeleportAssignment.Providers.UnitTests
{
    public enum ResponseMessageTypes
    {
        Success,
        Error,
        NotFound,
        NoSuchHost
    }

    public class CTeleportProviderTest
    {
        [Theory]
        [InlineData("AMS", ResponseMessageTypes.Success)]
        public async Task GetAirportInfoAsync_ExistedIataGiven_ReturnsAirtportInfo(string iata, ResponseMessageTypes messageType)
        {
            //Arrange
            string expected = "Netherland";
            var mockHttpClientFactory = new FakeHttpClientFactory().GetMockedClient(messageType);

            CTeleportProvider airportProvider = new CTeleportProvider(mockHttpClientFactory, new ProviderConfig());

            //Act
            var airportResponse = await airportProvider.GetAirportInfoByIataAsync(iata);
            
            //Assert
            Assert.Equal(expected, airportResponse.Value.Country);

        }

        [Theory]
        [InlineData("-dsfds", ResponseMessageTypes.Error)]
        public void GetAirportInfoAsync_IncorrectArgumentGiven_ThrowsInvalidCodeException(string iata, ResponseMessageTypes messageType)
        {
            //Arrange
            var mockHttpClientFactory = new FakeHttpClientFactory().GetMockedClient(messageType);
           
            CTeleportProvider airportProvider = new CTeleportProvider(mockHttpClientFactory, new ProviderConfig());

            //Act & Assert
            _ = Assert.ThrowsAsync<InvalidCodeException>(async () => await airportProvider.GetAirportInfoByIataAsync(iata));
        }



        [Theory]
        [InlineData("IFG", ResponseMessageTypes.NotFound)]
        public void GetAirportInfoAsync_NonExistanceArgumentGiven_ThrowsNotFoundException(string iata, ResponseMessageTypes messageType)
        {
            //Arrange
            var mockHttpClientFactory = new FakeHttpClientFactory().GetMockedClient(messageType);

            CTeleportProvider airportProvider = new CTeleportProvider(mockHttpClientFactory, new ProviderConfig());

            //Act & Assert
            _ = Assert.ThrowsAsync<NotFoundException>(async () => await airportProvider.GetAirportInfoByIataAsync(iata));
        }

        [Theory]
        [InlineData("RTR", ResponseMessageTypes.NoSuchHost)]
        public void GetAirportInfoAsync_IncorrectUrlGiven_ThrowsProviderException(string iata, ResponseMessageTypes messageType)
        {
            //Arrange
            var mockHttpClientFactory = new FakeHttpClientFactory().GetMockedClient(messageType);

            CTeleportProvider airportProvider = new CTeleportProvider(mockHttpClientFactory, new ProviderConfig());

            //Act & Assert
            _ = Assert.ThrowsAsync<ProviderException>(async () => await airportProvider.GetAirportInfoByIataAsync(iata));
        }
    }
}
