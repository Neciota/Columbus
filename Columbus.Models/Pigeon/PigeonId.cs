using System.Diagnostics.CodeAnalysis;

namespace Columbus.Models.Pigeon
{
    public readonly struct PigeonId : IEquatable<PigeonId>, IComparable<PigeonId>, ISpanParsable<PigeonId>
    {
        private PigeonId(CountryCode countryCode, int year, RingNumber ringNumber)
        {
            CountryCode = countryCode;
            Year = year;
            RingNumber = ringNumber;
        }

        private readonly CountryCode CountryCode { get; init; }
        private readonly int Year { get; init; }
        private readonly RingNumber RingNumber { get; init; }

        public static PigeonId Create(CountryCode countryCode, int year, RingNumber ringNumber)
        {
            return new PigeonId(countryCode, year, ringNumber);
        }

        public static PigeonId Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        {
            if (TryParse(s, provider, out PigeonId result))
                return result;

            throw new FormatException($"Invalid {nameof(PigeonId)} format.");
        }

        public static PigeonId Parse(string s, IFormatProvider? provider) => Parse(s.AsSpan(), provider);

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out PigeonId result)
        {
            result = default;
            if (!CountryCode.TryParse(s.Slice(0, 2), provider, out CountryCode countryCode))
                return false;

            if (!int.TryParse(s.Slice(2, 2), provider, out int year))
                return false;

            if (!RingNumber.TryParse(s.Slice(4), provider, out RingNumber ringNumber))
                return false;

            result = Create(countryCode, year, ringNumber);
            return true;
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out PigeonId result) => TryParse(s.AsSpan(), provider, out result);

        public int CompareTo(PigeonId other)
        {
            int countryCompare = CountryCode.CompareTo(other.CountryCode);
            if (countryCompare != 0)
                return countryCompare;

            int yearCompare = Year.CompareTo(other.Year);
            if (yearCompare != 0)
                return yearCompare;

            return RingNumber.CompareTo(other.RingNumber);
        }

        public bool Equals(PigeonId other)
        {
            return CountryCode.Equals(other.CountryCode) && Year.Equals(other.Year) && RingNumber.Equals(other.RingNumber);
        }

        public override string ToString()
        {
            return $"{CountryCode}{Year % 100:D2}-{RingNumber}";
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add(CountryCode);
            hashCode.Add(Year);
            hashCode.Add(RingNumber);
            return hashCode.ToHashCode();
        }

        public override bool Equals(object? obj)
        {
            return obj is PigeonId ring && Equals(ring);
        }

        public static bool operator ==(PigeonId left, PigeonId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PigeonId left, PigeonId right)
        {
            return !(left == right);
        }

        public static bool operator <(PigeonId left, PigeonId right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(PigeonId left, PigeonId right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(PigeonId left, PigeonId right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(PigeonId left, PigeonId right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}
