namespace CTeleportAssignment.Providers.UnitTests.Factories
{
    public class FakeHttpClientFactory : FakeHttpClientFactoryBase
    {
        public override Uri GetBaseAddress()
        {
            return new Uri("https://www.fake.testing/");
        }

        public override HttpResponseMessage GetResponse(ResponseMessageTypes messageType)
        {
            return HttpResponseFactory.FromCode(messageType);
        }
    }
}
