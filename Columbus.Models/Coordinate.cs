using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Columbus.Models
{
    /// <summary>
    /// Class <see cref="Coordinate"/> models a decimal degrees (DD) coordinate on the globe.
    /// </summary>
    /// <remarks>
    /// Create a <see cref="Coordinate"/> from a DD lattitude and DD longitude.
    /// </remarks>
    public struct Coordinate(double lattitude, double longitude) : IEquatable<Coordinate>, IComparable<Coordinate>, ISpanParsable<Coordinate>
    {
        /// <summary>
        /// Represent the longitude (east-west) of the coordinate.
        /// </summary>
        public double Longitude { get; private set; } = longitude;

        /// <summary>
        /// Represent the lattitude (north-south) of the coordinate.
        /// </summary>
        public double Lattitude { get; private set; } = lattitude;

        public readonly override string ToString()
        {
            return $"({Math.Round(Lattitude, 3)}, {Math.Round(Longitude, 3)})";
        }

        public readonly double GetDistance(Coordinate other)
        {
            const double K1 = Math.PI / 180;
            const double F1 = 0.99664718933525;
            const double B1 = 6378137;
            const double E1 = 0.0067394967422767;

            double B2 = (Longitude + other.Longitude) / 2;
            double L1 = Lattitude - other.Lattitude;
            double N1 = E1 * Math.Pow(Math.Cos(B2 * K1), 2);
            double V1 = Math.Pow(1 + N1, 0.5);
            double V2 = B1 / V1;
            double L2 = L1 * V1;

            double W1 = Math.Atan(F1 * Math.Tan(Longitude * K1));
            double W2 = Math.Atan(F1 * Math.Tan(other.Longitude * K1));

            double S1 = Math.Sin(W1) * Math.Sin(W2) + Math.Cos(W1) * Math.Cos(W2) * Math.Cos(L2 * K1);
            return Math.Round((Math.Atan(-S1 / Math.Sqrt(-S1 * S1 + 1)) + 2 * Math.Atan(1)) * V2, 3);
        }

        public readonly int CompareTo(Coordinate other)
        {
            int lattitudeComparison = Lattitude.CompareTo(other.Lattitude);
            if (lattitudeComparison != 0)
                return lattitudeComparison;

            return Longitude.CompareTo(other.Longitude);
        }

        public readonly bool Equals(Coordinate other)
        {
            return CompareTo(other) == 0;
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is Coordinate coordinate && Equals(coordinate);
        }

        public override readonly int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add(Longitude);
            hashCode.Add(Lattitude);

            return hashCode.ToHashCode();
        }

        public static Coordinate Parse(string s, IFormatProvider? provider)
        {
            return Parse(s.AsSpan(), provider);
        }

        public static Coordinate Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
        {
            if (TryParse(s, provider, out Coordinate coordinate))
                return coordinate;

            throw new FormatException($"Input string '{s}' was not in a correct format.");
        }

        public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Coordinate result)
        {
            return TryParse(s.AsSpan(), provider, out result);
        }

        public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Coordinate result)
        {
            result = default;

            int commaIndex = s.IndexOf(',');

            if (commaIndex < 0)
                return false;

            var lattitudeSpan = s[..commaIndex].Trim();
            var longitudeSpan = s[(commaIndex + 1)..].Trim();

            if (double.TryParse(lattitudeSpan, NumberStyles.Float, provider, out var lattitude) &&
                double.TryParse(longitudeSpan, NumberStyles.Float, provider, out var longitude))
            {
                result = new Coordinate(lattitude, longitude);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Convert a degree-minute-second format without separators into a decimal degrees format.
        /// </summary>
        public static Coordinate ParseFromDms(ReadOnlySpan<char> s, IFormatProvider? provider)
        {
            if (TryParseFromDms(s, provider, out Coordinate coordinate))
                return coordinate;

            throw new FormatException($"Input string '{s}' was not in a correct format.");
        }

        public static bool TryParseFromDms([NotNullWhen(true)] ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Coordinate result)
        {
            result = default;

            if (s.Length != 20)
                return false;

            const int componentLength = 10;
            var lattitudeSpan = s.Slice(0, componentLength);
            var longitudeSpan = s.Slice(10, componentLength);

            if (TryParseDmsToComponent(lattitudeSpan, provider, out var lattitude) &&
                TryParseDmsToComponent(longitudeSpan, provider, out var longitude))
            {
                result = new Coordinate(lattitude, longitude);
                return true;
            }

            return false;
        }

        private static bool TryParseDmsToComponent(ReadOnlySpan<char> s, IFormatProvider? provider, out double component)
        {
            component = default;

            if (double.TryParse(s.Slice(1, 2), provider, out double degrees) &&
                double.TryParse(s.Slice(3, 2), provider, out double minutes) &&
                double.TryParse(s[5..], provider, out double seconds))
            {
                int multiplier = s[0] == '-' ? -1 : 1;

                component = multiplier * (degrees + minutes / 60 + seconds / 3600);
                return true;
            }

            return false;
        }

        public static bool operator ==(Coordinate left, Coordinate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Coordinate left, Coordinate right)
        {
            return !(left == right);
        }

        public static bool operator <(Coordinate left, Coordinate right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator <=(Coordinate left, Coordinate right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >(Coordinate left, Coordinate right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator >=(Coordinate left, Coordinate right)
        {
            return left.CompareTo(right) >= 0;
        }
    }
}
