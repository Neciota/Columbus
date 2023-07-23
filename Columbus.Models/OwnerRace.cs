using System.Text.Json.Serialization;

namespace Columbus.Models
{
    /// <summary>
    /// Class <c>OwnerRace</c> models an owner's race participation.
    /// </summary>
    public class OwnerRace
    {
        /// <summary>
        /// Create an <c>OwnerRace</c> from Owner and race location.
        /// </summary>
        public OwnerRace(Owner owner, Coordinate startLocation, int count, int points)
        {
            Owner = owner;
            Distance = CalculateDistance(startLocation);
            Count = count;
            Points = points;
        }

        /// <summary>
        /// Create an <c>OwnerRace</c> from Owner and properties.
        /// </summary>
        [JsonConstructor]
        public OwnerRace(Owner owner, double distance, int count, int points)
        {
            Owner = owner;
            Distance = distance;
            Count = count;
            Points = points;
        }

        /// <summary>
        /// Represent the owner of the pigeons.
        /// </summary>
        public Owner Owner { get; private set; }

        /// <summary>
        /// Represent the distance between race start and owner location.
        /// </summary>
        public double Distance { get; private set; }

        /// <summary>
        /// Represent the amount of pigeons of the owner in the race.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Represent the points of an owner in a championship.
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// NPO-provided way of calculating race start-loft distance (arc length).
        /// </summary>
        private double CalculateDistance(Coordinate location)
        {
            if (Owner.Coordinate is null)
                throw new NullReferenceException($"{nameof(Owner.Coordinate)} cannot be null when calculating distance.");

            const double K1 = Math.PI / 180;
            const double F1 = 0.99664718933525;
            const double B1 = 6378137;
            const double E1 = 0.0067394967422767;

            double B2 = (location.Longitude + Owner.Coordinate.Longitude) / 2;
            double L1 = location.Lattitude - Owner.Coordinate.Lattitude;
            double N1 = E1 * Math.Pow(Math.Cos(B2 * K1), 2);
            double V1 = Math.Pow(1 + N1, 0.5);
            double V2 = B1 / V1;
            double L2 = L1 * V1;

            double W1 = Math.Atan(F1 * Math.Tan(location.Longitude * K1));
            double W2 = Math.Atan(F1 * Math.Tan(Owner.Coordinate.Longitude * K1));

            double S1 = Math.Sin(W1) * Math.Sin(W2) + Math.Cos(W1) * Math.Cos(W2) * Math.Cos(L2 * K1);
            return Math.Round((Math.Atan(-S1 / Math.Sqrt(-S1 * S1 + 1)) + 2 * Math.Atan(1)) * V2, 3);
        }
    }
}
