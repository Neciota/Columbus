using System.Globalization;
using System.Numerics;

namespace Columbus.Models
{
    public readonly struct ClubId :
        IComparable<ClubId>,
        IEquatable<ClubId>,
        ISpanParsable<ClubId>,
        IMinMaxValue<ClubId>
    {
        public static ClubId MinValue => new(0000);
        public static ClubId MaxValue => new(9999);

        public int Value { get; }

        private ClubId(int value)
        {
            Value = value;
        }

        public static ClubId Create(int value)
        {
            ClubId clubId = new(value);

            ArgumentOutOfRangeException.ThrowIfLessThan(clubId, MinValue);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(clubId, MaxValue);

            return clubId;
        }

        public int CompareTo(ClubId other) => Value.CompareTo(other.Value);

        public bool Equals(ClubId other) => Value == other.Value;

        public override bool Equals(object? obj) => obj is ClubId other && Equals(other);

        public override int GetHashCode() => Value;

        public override string ToString() => Value.ToString("D4", CultureInfo.InvariantCulture);

        public static bool operator ==(ClubId left, ClubId right) => left.Equals(right);

        public static bool operator !=(ClubId left, ClubId right) => !left.Equals(right);

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out ClubId result)
        {
            if (int.TryParse(s, NumberStyles.None, provider, out int value))
            {
                result = Create(value);

                return true;
            }

            result = default;
            return false;
        }

        public static bool TryParse(string? s, IFormatProvider? provider, out ClubId result) =>
            TryParse(s.AsSpan(), provider, out result);

        public static ClubId Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        {
            if (TryParse(s, provider, out ClubId result))
                return result;

            throw new FormatException($"Invalid ClubId format. Must be an integer between {MinValue} and {MaxValue}.");
        }

        public static ClubId Parse(string s, IFormatProvider? provider) => Parse(s.AsSpan(), provider);

        public static bool operator <(ClubId left, ClubId right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(ClubId left, ClubId right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(ClubId left, ClubId right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(ClubId left, ClubId right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}
