using CTeleportAssignment.Providers.Config;
using CTeleportAssignment.Providers.Exceptions;
using CTeleportAssignment.Providers.Extensions;
using CTeleportAssignment.Providers.Infrastructure;
using CTeleportAssignment.Providers.Models;
using System.Net;
using System.Text.Json;

namespace CTeleportAssignment.Providers
{
    public class CTeleportProvider : IAirportProvider
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ProviderConfig _providerConfig;
        public CTeleportProvider(IHttpClientFactory httpClientFactory
            , ProviderConfig providerConfig)
        {
            _httpClient = httpClientFactory;
            _providerConfig = providerConfig;
        }

        public async Task<AirportInfo?> GetAirportInfoByIataAsync(string iata)
        {
            string url = new UrlBuilder(_providerConfig.Url)
                .AddSegment(iata)
                .Build();

            try
            {
                var httpResponseMessage = await _httpClient.SendAsync(url, HttpMethod.Get);

                if (httpResponseMessage.StatusCode == HttpStatusCode.BadRequest)
                    throw new InvalidCodeException(iata);

                if (httpResponseMessage.StatusCode == HttpStatusCode.NotFound)
                    throw new NotFoundException(iata);

                httpResponseMessage.EnsureSuccessStatusCode();

                var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();

                return await JsonSerializer.DeserializeAsync
                    <AirportInfo>(contentStream);
            }
            catch (Exception exp) when (!(exp is InvalidCodeException || exp is NotFoundException))
            {
                throw new ProviderException(exp.Message);
            }

        }
    }
}
