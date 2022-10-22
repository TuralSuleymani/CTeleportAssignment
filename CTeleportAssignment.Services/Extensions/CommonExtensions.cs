namespace CTeleportAssignment.Services.Extensions
{
    public static class CommonExtensions
    {
        public static string AsMiles(this double inMetersValue)
        {
            return (inMetersValue * 0.000621371192).ToString("#.##");
        }
    }
}
