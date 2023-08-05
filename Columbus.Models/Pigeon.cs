namespace Columbus.Models
{
    /// <summary>
    /// Class <c>Pigeon</c> models a racing pigeon.
    /// </summary>
    public class Pigeon
    {
        /// <summary>
        /// Create an <c>Pigeon</c> from ownership data.
        /// </summary>
        public Pigeon(string country, int year, int ringNumber, int chip, Sex sex)
        {
            Country = country;
            Year = year;
            RingNumber = ringNumber;
            Chip = chip;
            Sex = sex;
        }

        public int Year { get; private set; }

        public string Country { get; private set; }

        public int RingNumber { get; private set; }

        public int Chip { get; set; }

        public Sex Sex { get; private set; }

        public override string ToString()
        {
            return $"{Country}{GetTwoDigitYear()}-{RingNumber}";
        }

        public string GetTwoDigitYear()
        {
            return $"{Year - 2000}";
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(Country);
            hash.Add(Year);
            hash.Add(RingNumber);

            return hash.ToHashCode();
        }

        public override bool Equals(object? obj)
        {
            return (obj?.GetHashCode() ?? 0) == this.GetHashCode();
        }

        public static bool operator ==(Pigeon? a, Pigeon? b)
        {
            if (a is null && b is null)
                return true;
            else if (a is null && b is not null)
                return false;

            return a!.Equals(b);
        }

        public static bool operator !=(Pigeon? a, Pigeon? b)
        {
            if (a is null && b is null)
                return true;
            else if (a is null && b is not null)
                return false;

            return !a!.Equals(b);
        }
    }
}
