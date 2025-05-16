namespace Columbus.Models.Race
{
    using Columbus.Models.Owner;

    /// <summary>
    /// Class <c>OwnerRace</c> models an owner's race participation.
    /// </summary>
    public class OwnerRace
    {
        /// <summary>
        /// Create an <c>OwnerRace</c> from Owner and race location.
        /// </summary>
        public OwnerRace(Owner owner, Coordinate startLocation, int count, TimeSpan clockDeviation)
        {
            Owner = owner;
            Distance = CalculateDistance(startLocation);
            Count = count;
            ClockDeviation = clockDeviation;
        }

        /// <summary>
        /// Create an <c>OwnerRace</c> from Owner and properties.
        /// </summary>
        public OwnerRace(Owner owner, double distance, int count, TimeSpan clockDeviation)
        {
            Owner = owner;
            Distance = distance;
            Count = count;
            ClockDeviation = clockDeviation;
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
        /// By how much time the clock deviated between submission and stopping.
        /// Positive means the owner clock lagged behind (owner clock time lies before atomic time).
        /// Negative means the owner clock jumped ahead (owner clock lies ahead of the atomic time).
        /// </summary>
        public TimeSpan ClockDeviation { get; private set; }

        /// <summary>
        /// NPO-provided way of calculating race start-loft distance (arc length).
        /// </summary>
        private double CalculateDistance(Coordinate location) => Owner.LoftCoordinate.GetDistance(location);
    }
}
