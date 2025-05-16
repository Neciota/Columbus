namespace Columbus.Models.Pigeon
{
    /// <summary>
    /// Class <c>Pigeon</c> models a racing pigeon.
    /// </summary>
    public class Pigeon
    {
        /// <summary>
        /// Create an <c>Pigeon</c> from ownership data.
        /// </summary>
        public Pigeon(CountryCode country, int year, RingNumber ringNumber, int chip, Sex sex)
        {
            Country = country;
            Year = year;
            RingNumber = ringNumber;
            Chip = chip;
            Sex = sex;
        }

        public int Year { get; private set; }

        public CountryCode Country { get; private set; }

        public RingNumber RingNumber { get; private set; }

        public int Chip { get; set; }

        public Sex Sex { get; private set; }

        public override string ToString()
        {
            return $"{Country}{GetTwoDigitYear()}-{RingNumber}";
        }

        public int GetTwoDigitYear()
        {
            return Year % 100;
        }

        public override int GetHashCode()
        {
            HashCode hash = new();
            hash.Add(Country);
            hash.Add(Year);
            hash.Add(RingNumber);

            return hash.ToHashCode();
        }

        public override bool Equals(object? obj)
        {
            return (obj?.GetHashCode() ?? 0) == GetHashCode();
        }
    }
}
