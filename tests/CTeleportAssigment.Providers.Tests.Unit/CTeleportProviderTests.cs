using System.Net;
using System.Text.Json;
using CTeleportAssigment.Providers.Tests.Unit.Extensions;
using CTeleportAssignment.Providers;
using CTeleportAssignment.Providers.Config;
using CTeleportAssignment.Providers.Exceptions;
using CTeleportAssignment.Providers.Models;
using CTeleportAssignment.Tests.Data;
using FluentAssertions;
using Moq;

namespace CTeleportAssigment.Providers.Tests.Unit
{

    public class CTeleportProviderTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly ProviderConfig _providerConfig;

        public CTeleportProviderTests()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _providerConfig = new ProviderConfig("https://api.cteleport.com");
        }
        [Fact]
        public async Task GetAirportInfoByIataAsync_WhenResponseIsSuccessful_ShouldReturnAirportInfo()
        {
            // Arrange
            
            var expectedAirportInfo = AirportInfoData.AirportInfo();
            var iata = expectedAirportInfo.Iata;
            var jsonResponse = JsonSerializer.Serialize(expectedAirportInfo);

            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(jsonResponse)
            };

            _httpMessageHandlerMock.SetupFromHttpMessage(
              req => req.Method == HttpMethod.Get && req.RequestUri!.ToString().Contains(iata),
              responseMessage);

            var provider = CreateProvider();

            // Act
            var result = await provider.GetAirportInfoByIataAsync(iata);

            // Assert
            result.Should().NotBeNull();
            result!.Iata.Should().Be(iata);
            result.Name.Should().Be(expectedAirportInfo.Name);
        }


        [Fact]
        public async Task GetAirportInfoByIataAsync_WhenResponseIsBadRequest_ShouldThrowInvalidCodeException()
        {
            // Arrange
            var iata = AirportInfoData.InvalidIata;

            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest
            };

            _httpMessageHandlerMock.SetupFromHttpMessage(
              req => req.Method == HttpMethod.Get && req.RequestUri!.ToString().Contains(iata),
              responseMessage);

            var provider = CreateProvider();

            // Act
            var act = async () => await provider.GetAirportInfoByIataAsync(iata);

            // Assert
            await act.Should().ThrowAsync<InvalidCodeException>()
                .WithMessage($"*{iata}*");
        }

        [Fact]
        public async Task GetAirportInfoByIataAsync_WhenResponseIsNotFound_ShouldThrowNotFoundException()
        {
            // Arrange
            var iata = AirportInfoData.FirstIata;

            var responseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            };

            _httpMessageHandlerMock.SetupFromHttpMessage(
              req => req.Method == HttpMethod.Get && req.RequestUri!.ToString().Contains(iata),
              responseMessage);

            var provider = CreateProvider();

            // Act
            var act = async () => await provider.GetAirportInfoByIataAsync(iata);

            // Assert
            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"*{iata}*");
        }

        [Fact]
        public async Task GetAirportInfoByIataAsync_WhenUnhandledExceptionOccurs_ShouldThrowProviderException()
        {
            // Arrange
            var iata = AirportInfoData.FirstIata;

            var requestException = new HttpRequestException("Unexpected error");

            _httpMessageHandlerMock.SetupFromException(
              req => req.Method == HttpMethod.Get && req.RequestUri!.ToString().Contains(iata),
              requestException);
            var provider = CreateProvider();

            // Act
            var act = async () => await provider.GetAirportInfoByIataAsync(iata);

            // Assert
            await act.Should().ThrowAsync<ProviderException>()
                .WithMessage("Unexpected error");
        }

        private CTeleportProvider CreateProvider()
        {
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _httpClientFactoryMock.Setup(factory => factory.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            return new CTeleportProvider(_httpClientFactoryMock.Object, _providerConfig);
        }
    }

}
