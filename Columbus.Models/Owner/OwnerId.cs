using System.Globalization;
using System.Numerics;

namespace Columbus.Models.Owner
{
    public readonly struct OwnerId : IEquatable<OwnerId>, IComparable<OwnerId>, IMinMaxValue<OwnerId>, ISpanParsable<OwnerId>
    {
        private OwnerId(int id)
        {
            Value = id;
        }

        public readonly int Value { get; init; }

        public static OwnerId MaxValue => new(9999_9999);

        public static OwnerId MinValue => new(0000_0000);

        public static OwnerId Create(int id)
        {
            OwnerId ownerId = new(id);

            ArgumentOutOfRangeException.ThrowIfLessThan(ownerId, MinValue);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(ownerId, MaxValue);

            return ownerId;
        }

        public override string ToString() => Value.ToString();

        public int CompareTo(OwnerId other)
        {
            return Value.CompareTo(other.Value);
        }

        public override bool Equals(object? obj)
        {
            return obj is OwnerId id && Equals(id);
        }

        public bool Equals(OwnerId other)
        {
            return CompareTo(other) == 0;
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add(Value);

            return hashCode.ToHashCode();
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, out OwnerId result)
        {
            if (int.TryParse(s, NumberStyles.None, provider, out int value))
            {
                result = Create(value);

                return true;
            }

            result = default;
            return false;
        }

        public static bool TryParse(string? s, IFormatProvider? provider, out OwnerId result) =>
            TryParse(s.AsSpan(), provider, out result);

        public static OwnerId Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        {
            if (TryParse(s, provider, out OwnerId result))
                return result;

            throw new FormatException($"Invalid ClubId format. Must be an integer between {MinValue} and {MaxValue}.");
        }

        public static OwnerId Parse(string s, IFormatProvider? provider) => Parse(s.AsSpan(), provider);

        public static bool operator ==(OwnerId left, OwnerId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(OwnerId left, OwnerId right)
        {
            return !(left == right);
        }

        public static bool operator <(OwnerId left, OwnerId right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(OwnerId left, OwnerId right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(OwnerId left, OwnerId right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(OwnerId left, OwnerId right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}
