using Moq;
using Moq.Protected;

namespace CTeleportAssigment.Providers.Tests.Unit.Extensions
{
    public static class HttpMessageHandlerMockExtensions
    {
        private static Moq.Language.Flow.ISetup<HttpMessageHandler, Task<HttpResponseMessage>> SetupHttpMessageHandler(
            Mock<HttpMessageHandler> mock,
            Func<HttpRequestMessage, bool> requestCondition)
        {
            return mock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => requestCondition(req)),
                    ItExpr.IsAny<CancellationToken>());
        }

        public static void SetupFromHttpMessage(
            this Mock<HttpMessageHandler> mock,
            Func<HttpRequestMessage, bool> requestCondition,
            HttpResponseMessage responseMessage)
        {
            if (responseMessage != null)
                SetupHttpMessageHandler(mock, requestCondition)
                    .ReturnsAsync(responseMessage);
            else
                throw new ArgumentNullException(nameof(responseMessage));
        }

        public static void SetupFromException(
            this Mock<HttpMessageHandler> mock,
            Func<HttpRequestMessage, bool> requestCondition,
            Exception exception)
        {
            if (exception != null)
                SetupHttpMessageHandler(mock, requestCondition)
                    .ThrowsAsync(exception);
            else
                throw new ArgumentNullException(nameof(exception));
        }
    }

}
