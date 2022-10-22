using System.Text.Json.Serialization;

namespace CTeleportAssignment.Providers.Models
{
    [Serializable]
    public class AirportInfo
    {
        [JsonPropertyName("country")]
        public string Country { get; set; }

        [JsonPropertyName("city_iata")]
        public string CityIata { get; set; }


        [JsonPropertyName("iata")]
        public string Iata { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; }

        [JsonPropertyName("timezone_region_name")]
        public string TimeZoneRegionName { get; set; }

        [JsonPropertyName("country_iata")]
        public string CountryIata { get; set; }

        [JsonPropertyName("rating")]
        public int Rating { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
        
        [JsonPropertyName("location")]
        public Location Location { get; set; }
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("hubs")]
        public int Hubs { get; set; }

    }
}
