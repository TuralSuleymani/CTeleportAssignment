using Moq;
using Moq.Protected;

namespace CTeleportAssignment.Providers.UnitTests.Factories
{
    public abstract class FakeHttpClientFactoryBase
    {
        public abstract HttpResponseMessage GetResponse(ResponseMessageTypes messageType);

        public abstract Uri GetBaseAddress();

        public IHttpClientFactory GetMockedClient(ResponseMessageTypes messageType)
        {
            var response = GetResponse(messageType);

            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

            handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .Returns(Task.FromResult(response))
            .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = GetBaseAddress()
            };

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();

            mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            return mockHttpClientFactory.Object;

        }
    }
}
