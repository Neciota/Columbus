using System.Buffers.Text;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Numerics;

namespace Columbus.Models.Race
{
    public readonly struct Speed(double metersPerSecond) :
        IComparable<Speed>,
        IEquatable<Speed>,
        ISpanParsable<Speed>,
        IUtf8SpanParsable<Speed>,
        IAdditionOperators<Speed, Speed, Speed>,
        ISubtractionOperators<Speed, Speed, Speed>,
        IMultiplyOperators<Speed, double, Speed>,
        IDivisionOperators<Speed, double, Speed>,
        IEqualityOperators<Speed, Speed, bool>
    {
        private readonly double _metersPerSecond = metersPerSecond;

        public double MetersPerSecond => _metersPerSecond;
        public double KilometersPerHour => _metersPerSecond * 3.6;
        public double MetersPerMinute => _metersPerSecond * 60;

        public static readonly Speed Zero = new(0);

        public int CompareTo(Speed other) => _metersPerSecond.CompareTo(other._metersPerSecond);

        public bool Equals(Speed other) => _metersPerSecond.Equals(other._metersPerSecond);

        public override bool Equals(object? obj) => obj is Speed other && Equals(other);

        public override int GetHashCode() => _metersPerSecond.GetHashCode();

        public override string ToString() => _metersPerSecond.ToString(CultureInfo.InvariantCulture);

        public static Speed operator +(Speed left, Speed right) => new(left._metersPerSecond + right._metersPerSecond);

        public static Speed operator -(Speed left, Speed right) => new(left._metersPerSecond - right._metersPerSecond);

        public static Speed operator *(Speed left, double right) => new(left._metersPerSecond * right);

        public static Speed operator *(double left, Speed right) => new(left * right._metersPerSecond);

        public static Speed operator /(Speed left, double right) => new(left._metersPerSecond / right);

        public static bool operator ==(Speed left, Speed right) => left.Equals(right);

        public static bool operator !=(Speed left, Speed right) => !left.Equals(right);

        // IParsable / ISpanParsable
        public static Speed Parse(string s, IFormatProvider? provider) =>
            new(double.Parse(s, provider));

        public static Speed Parse(ReadOnlySpan<char> s, IFormatProvider? provider) =>
            new(double.Parse(s, provider));

        public static bool TryParse(string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Speed result)
        {
            if (double.TryParse(s, provider, out var value))
            {
                result = new Speed(value);
                return true;
            }
            result = default;
            return false;
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Speed result)
        {
            if (double.TryParse(s, provider, out var value))
            {
                result = new Speed(value);
                return true;
            }
            result = default;
            return false;
        }

        // IUtf8SpanParsable
        public static Speed Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider)
        {
            if (Utf8Parser.TryParse(utf8Text, out double value, out _))
            {
                return new Speed(value);
            }
            throw new FormatException("Invalid UTF-8 input for Speed.");
        }

        public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, [MaybeNullWhen(false)] out Speed result)
        {
            if (Utf8Parser.TryParse(utf8Text, out double value, out _))
            {
                result = new Speed(value);
                return true;
            }
            result = default;
            return false;
        }

        public static bool operator <(Speed left, Speed right) => left.CompareTo(right) < 0;
        public static bool operator <=(Speed left, Speed right) => left.CompareTo(right) <= 0;
        public static bool operator >(Speed left, Speed right) => left.CompareTo(right) > 0;
        public static bool operator >=(Speed left, Speed right) => left.CompareTo(right) >= 0;
    }
}
