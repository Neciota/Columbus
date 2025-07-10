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
            Id = PigeonId.Create(country, year, ringNumber);
            Chip = chip;
            Sex = sex;
        }

        public Pigeon(PigeonId pigeonId, int chip, Sex sex)
        {
            Id = pigeonId;
            Chip = chip;
            Sex = sex;
        }

        public PigeonId Id { get; set; }

        public int Chip { get; set; }

        public Sex Sex { get; private set; }

        public override string ToString() => Id.ToString();

        public override int GetHashCode()
        {
            HashCode hash = new();
            hash.Add(Id);
            return hash.ToHashCode();
        }

        public override bool Equals(object? obj)
        {
            return (obj?.GetHashCode() ?? 0) == GetHashCode();
        }
    }
}
