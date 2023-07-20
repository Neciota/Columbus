using System.Text.Json.Serialization;

namespace Columbus.Models
{
    /// <summary>
    /// Class <c>Coordinate</c> models a decimal degrees (DD) coordinate on the globe.
    /// </summary>
    public class Coordinate
    {
        /// <summary>
        /// Create a <c>Coordinate</c> from a DMS lattitude and DMS longitude.
        /// </summary>
        public Coordinate(string longitude, string lattitude)
        {
            Longitude = DMStoDD(longitude);
            Lattitude = DMStoDD(lattitude);
        }

        /// <summary>
        /// Create a <c>Coordinate</c> from a DD lattitude and DD longitude.
        /// </summary>
        [JsonConstructorAttribute]
        public Coordinate(double longitude, double lattitude)
        {
            Longitude = longitude;
            Lattitude = lattitude;
        }

        /// <summary>
        /// Represent the longitude (north-south) of the coordinate.
        /// </summary>
        public double Longitude { get; private set; }

        /// <summary>
        /// Represent the lattitude (east-west) of the coordinate.
        /// </summary>
        public double Lattitude { get; private set; }

        /// <summary>
        /// Convert a degree-minute-second format without separators into a decimal degrees format.
        /// </summary>
        public static double DMStoDD(string coordinate)
        {
            double degrees = Convert.ToDouble(coordinate.Substring(0, 2));
            double minutes = Convert.ToDouble(coordinate.Substring(2, 2)) / 60;
            double seconds = Convert.ToDouble(coordinate.Substring(4, 2)) / 3600;
            double decimalSeconds = Convert.ToDouble(coordinate.Substring(7)) / 100 / 3600;
            return degrees + minutes + seconds + decimalSeconds;
        }

        public override string ToString()
        {
            return $"({Math.Round(Longitude, 3)}, {Math.Round(Lattitude)})";
        }
    }
}
