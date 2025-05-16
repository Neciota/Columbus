using System.Diagnostics.CodeAnalysis;

namespace Columbus.Models.Pigeon
{
    public readonly struct CountryCode : IEquatable<CountryCode>, IComparable<CountryCode>, ISpanParsable<CountryCode>
    {
        private CountryCode(string code)
        {
            Value = code;
        }

        public static CountryCode Create(string code)
        {
            CountryCode countryCode = new(code.Trim());

            ArgumentException.ThrowIfNullOrWhiteSpace(countryCode.Value);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(countryCode.Value.Length, 2);

            return countryCode;
        }

        public readonly string Value { get; init; }

        public override string ToString() => Value;

        public int CompareTo(CountryCode other)
        {
            return Value.CompareTo(other.Value);
        }

        public bool Equals(CountryCode other)
        {
            return CompareTo(other) == 0;
        }

        public override bool Equals(object? obj)
        {
            return obj is CountryCode country && Equals(country);
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add(Value);
            return hashCode.ToHashCode();
        }

        public static CountryCode Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        {
            return Create(s.ToString());
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out CountryCode result)
        {
            result = default;

            try
            {
                result = Create(s.ToString());
                return true;
            } 
            catch (ArgumentException)
            {
                return false;
            }
        }

        public static CountryCode Parse(string s, IFormatProvider? provider)
        {
            return Parse(s.AsSpan(), provider);
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out CountryCode result)
        {
            return TryParse(s.AsSpan(), provider, out result);
        }

        public static bool operator ==(CountryCode left, CountryCode right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CountryCode left, CountryCode right)
        {
            return !(left == right);
        }

        public static bool operator <(CountryCode left, CountryCode right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(CountryCode left, CountryCode right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(CountryCode left, CountryCode right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(CountryCode left, CountryCode right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}
