using System.Net;
using System.Text.Json;
using CTeleportAssignment.Providers.Models;

namespace CTeleportAssignment.Providers.UnitTests.Factories
{
    public static class HttpResponseFactory
    {
        private static AirportInfo _GetAirportInfo()
        {
            return new AirportInfo
            {
                City = "Rotterdam",
                Country = "Netherland",
                Rating = 5,
                Location = new Location
                {
                    Lat = 45.5677,
                    Lon = 67.6744
                }
            };
        }
        public static HttpResponseMessage FromCode(ResponseMessageTypes messageType)
        {
            StringContent content = null;

            HttpStatusCode statusCode = HttpStatusCode.OK;

            if (messageType == ResponseMessageTypes.Success)
            {
                content = new StringContent(JsonSerializer.Serialize(_GetAirportInfo()));
            }
            else if (messageType == ResponseMessageTypes.Error)
            {
                var error = "{\"errors\":[{\"value\":\"-fgdssdf\",\"msg\":\"Invalid code\",\"param\":\"code\",\"location\":\"params\"}]}";
                content = new StringContent(error);
                statusCode = HttpStatusCode.BadRequest;
            }
            else if (messageType == ResponseMessageTypes.NotFound)
            {
                content = new StringContent("Not Found");
                statusCode = HttpStatusCode.NotFound;
            }
            else if (messageType == ResponseMessageTypes.NoSuchHost)
            {
                content = new StringContent("No such host is known");
                statusCode = HttpStatusCode.InternalServerError;
            }
            return new HttpResponseMessage { StatusCode = statusCode, Content = content };
        }
    }
}
