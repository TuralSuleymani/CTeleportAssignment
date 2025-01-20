namespace CTeleportAssignment.Providers.Extensions
{
    public static class IHttpClientFactoryExtensions
    {
        public static async Task<HttpResponseMessage> SendAsync(this IHttpClientFactory httpClientFactory , string url, HttpMethod method)
        {
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            var httpClient = httpClientFactory.CreateClient();

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}
