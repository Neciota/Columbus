using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Columbus.Models.Race
{
    public readonly struct RaceType : IEquatable<RaceType>, IComparable<RaceType>, ISpanParsable<RaceType>, IMinMaxValue<RaceType>
    {
        private RaceType(char raceType)
        {
            Value = raceType;
        }

        public char Value { get; init; }

        public static RaceType MaxValue => new('Z');

        public static RaceType MinValue => new('A');

        public static RaceType Create(char value)
        {
            RaceType raceType = new(value);

            ArgumentOutOfRangeException.ThrowIfLessThan(raceType, MinValue);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(raceType, MaxValue);

            return raceType;
        }

        public override bool Equals(object? obj)
        {
            return obj is RaceType raceType && Equals(raceType);
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add(Value);
            return hashCode.ToHashCode();
        }

        public bool Equals(RaceType other)
        {
            return Value == other.Value;
        }

        public int CompareTo(RaceType other)
        {
            return Value.CompareTo(other.Value);
        }

        public static RaceType Parse(ReadOnlySpan<char> s, IFormatProvider? provider = null)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(s.Length, 1, nameof(s));
            ArgumentOutOfRangeException.ThrowIfGreaterThan(s.Length, 1, nameof(s));

            return Create(s[0]);
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out RaceType result)
        {
            result = default;

            try
            {
                result = Parse(s, provider);
                return true;
            } catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }

        public static RaceType Parse(string s, IFormatProvider? provider = null)
        {
            return Parse(s.AsSpan(), provider);
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out RaceType result)
        {
            result = default;

            try
            {
                result = Parse(s ?? string.Empty, provider);
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }
        }

        public static bool operator ==(RaceType left, RaceType right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RaceType left, RaceType right)
        {
            return !(left == right);
        }

        public static bool operator <(RaceType left, RaceType right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(RaceType left, RaceType right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(RaceType left, RaceType right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(RaceType left, RaceType right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}
