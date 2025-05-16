namespace Columbus.Models.Owner
{
    public readonly struct Address(string street, string postcode, string countryCode, string town) : IEquatable<Address>
    {
        public string Street { get; } = street;
        public string Postcode { get; } = postcode;
        public string CountryCode { get; } = countryCode;
        public string Town { get; } = town;

        public override string ToString() => $"{Street}, {Postcode} {Town}, {CountryCode}";

        public bool Equals(Address other) =>
            Street == other.Street &&
            Postcode == other.Postcode &&
            CountryCode == other.CountryCode &&
            Town == other.Town;

        public override bool Equals(object? obj) =>
            obj is Address other && Equals(other);

        public override int GetHashCode() =>
            HashCode.Combine(Street, Postcode, CountryCode, Town);

        public static bool operator ==(Address left, Address right) => left.Equals(right);
        public static bool operator !=(Address left, Address right) => !left.Equals(right);
    }
}
