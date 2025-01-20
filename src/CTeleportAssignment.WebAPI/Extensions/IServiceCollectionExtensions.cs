using CTeleportAssignment.Providers;
using Polly.Extensions.Http;
using Polly;
using CTeleportAssignment.Providers.Config;

namespace CTeleportAssignment.WebAPI.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCircuitBreaker(this IServiceCollection serviceCollection)
        {

            var circuitBreakerPolicy = GetCircuitBreakerPolicy();
            serviceCollection.AddHttpClient<IAirportProvider, CTeleportProvider>()
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                    .AddPolicyHandler(GetRetryPolicy())
                    .AddPolicyHandler(GetCircuitBreakerPolicy());

            static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
            {
                return HttpPolicyExtensions
                               .HandleTransientHttpError()
                               .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                               .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            }

            static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
            {
                return HttpPolicyExtensions
                               .HandleTransientHttpError()
                               .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                               .CircuitBreakerAsync(3, TimeSpan.FromSeconds(20));
            }
            return serviceCollection;
        }

        public static IServiceCollection AddApiServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAirportProvider, CTeleportProvider>();
            return serviceCollection;
        }

        public static IServiceCollection AddApiConfiguration(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var url = configuration["Provider:Url"];
            serviceCollection.AddSingleton(sp => new ProviderConfig(url!));
            return serviceCollection;
        }

    }
}
