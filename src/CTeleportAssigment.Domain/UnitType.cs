using Ardalis.SmartEnum;

namespace CTeleportAssignment.Services.Models
{
    public sealed class UnitType : SmartEnum<UnitType>
    {
        public static readonly UnitType Mile = new(nameof(Mile), 1);
        public static readonly UnitType Meter = new(nameof(Meter), 2);

        private UnitType(string name, int value) : base(name, value) { }
    }
}
