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
            return $"{Country}{Year}-{RingNumber}";
        }
    }
}
