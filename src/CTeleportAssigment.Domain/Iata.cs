namespace CTeleportAssigment.Domain
{
    public record Iata
    {
        private const int STRICT_IATA_LENGTH = 3;
        private static readonly string IATA_CODE_LENGTH_ERROR =
            $"IATA Code should be {STRICT_IATA_LENGTH} characters long.";
        public string Value { get; }

        private Iata(string iata)
        {
            Value = iata;
        }

        public static Iata Create(string iata)
        {
            if (string.IsNullOrWhiteSpace(iata) || iata.Length != STRICT_IATA_LENGTH)
            {
                throw new ArgumentException(IATA_CODE_LENGTH_ERROR, nameof(iata));
            }
            return new Iata(iata.ToUpper());
        }
    }
}
