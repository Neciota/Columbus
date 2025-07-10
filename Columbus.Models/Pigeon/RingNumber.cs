using System.Globalization;
using System.Numerics;

namespace Columbus.Models.Pigeon
{
    public readonly struct RingNumber : IEquatable<RingNumber>, IComparable<RingNumber>, IMinMaxValue<RingNumber>, ISpanParsable<RingNumber>
    {
        private RingNumber(int ringNumber)
        {
            Value = ringNumber;
        }

        public readonly int Value { get; init; }

        public static RingNumber MinValue => new(00_00_000);

        public static RingNumber MaxValue => new(99_99_999);

        public static RingNumber Create(int number)
        {
            RingNumber ringNumber = new(number);

            ArgumentOutOfRangeException.ThrowIfLessThan(ringNumber, MinValue);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(ringNumber, MaxValue);

            return ringNumber;
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public override bool Equals(object? obj)
        {
            return obj is RingNumber ringNumber && Equals(ringNumber);
        }

        public bool Equals(RingNumber other)
        {
            return CompareTo(other) == 0;
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add(Value);

            return hashCode.ToHashCode();
        }

        public int CompareTo(RingNumber other)
        {
            return Value.CompareTo(other.Value);
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out RingNumber result)
        {
            if (int.TryParse(s, NumberStyles.None, provider, out int value))
            {
                result = Create(value);

                return true;
            }

            result = default;
            return false;
        }

        public static bool TryParse(string? s, IFormatProvider? provider, out RingNumber result) =>
            TryParse(s.AsSpan(), provider, out result);

        public static RingNumber Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        {
            if (TryParse(s, provider, out RingNumber result))
                return result;

            throw new FormatException($"Invalid {nameof(RingNumber)} format. Must be an integer between {MinValue} and {MaxValue}.");
        }

        public static RingNumber Parse(string s, IFormatProvider? provider) => Parse(s.AsSpan(), provider);

        public static bool operator ==(RingNumber left, RingNumber right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RingNumber left, RingNumber right)
        {
            return !(left == right);
        }

        public static bool operator <(RingNumber left, RingNumber right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(RingNumber left, RingNumber right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(RingNumber left, RingNumber right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(RingNumber left, RingNumber right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}
