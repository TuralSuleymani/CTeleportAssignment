namespace CTeleportAssignment.Services.Models
{
    public sealed class Distance
    {
        private Distance(double value, UnitType unitType)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("value");
            }

            if (unitType is null)
            {
                throw new ArgumentException("unitType");
            }
            Value = value;
            UnitType = unitType;
        }

        public static Distance Mile(double value) => new(value, UnitType.Mile);
        public static Distance Meter(double value) => new(value, UnitType.Meter);

        public double Value { get; }
        public UnitType UnitType { get; }
    }
}
