namespace CTeleportAssigment.Domain
{
    public class Location
    {
        public double Latitude { get; }
        public double Longitude { get; }

        private Location(double latitude, double longitude)
        {
            if (!IsValidLatitude(latitude))
            {
                throw new ArgumentException($"Invalid latitude: {latitude}. Latitude must be between -90 and 90 degrees.");
            }

            if (!IsValidLongitude(longitude))
            {
                throw new ArgumentException($"Invalid longitude: {longitude}. Longitude must be between -180 and 180 degrees.");
            }

            Latitude = latitude;
            Longitude = longitude;
        }

        public static Location Create(double latitude, double longitude)
        {
            return new Location(latitude, longitude);
        }

        private static bool IsValidLatitude(double latitude) => latitude is >= -90 and <= 90;

        private static bool IsValidLongitude(double longitude) => longitude is >= -180 and <= 180;

        public double CalculateDistanceInMiles(Location other)
        {
            return CalculateDistance(other, EarthRadiusMiles);
        }

        public double CalculateDistanceInMeters(Location other)
        {
            return CalculateDistance(other, EarthRadiusMeters);
        }

        private double CalculateDistance(Location other, double earthRadius)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other), "Other location cannot be null.");
            }

            double lat1 = DegreesToRadians(Latitude);
            double lon1 = DegreesToRadians(Longitude);
            double lat2 = DegreesToRadians(other.Latitude);
            double lon2 = DegreesToRadians(other.Longitude);

            double dLat = lat2 - lat1;
            double dLon = lon2 - lon1;

            double a = Math.Pow(Math.Sin(dLat / 2), 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(dLon / 2), 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return earthRadius * c;
        }

        private static double DegreesToRadians(double degrees) => degrees * Math.PI / 180;

        public override string ToString() => $"Latitude: {Latitude}, Longitude: {Longitude}";

        public override bool Equals(object? obj)
        {
            if (obj is not Location other) return false;
            return Latitude == other.Latitude && Longitude == other.Longitude;
        }

        public override int GetHashCode() => HashCode.Combine(Latitude, Longitude);

        private const double EarthRadiusMiles = 3958.8;
        private const double EarthRadiusMeters = 6371e3; // 6371 km = 6371 * 1000 meters
    }


}
