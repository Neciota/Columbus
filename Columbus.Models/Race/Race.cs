namespace Columbus.Models.Race
{
    using Columbus.Models.Owner;

    public class Race
    {
        public Race(
            int number,
            RaceType type,
            string name,
            string code,
            DateTime startTime,
            Coordinate location,
            IList<OwnerRace> ownerRaces,
            IList<PigeonRace> pigeonRaces,
            int pointsQuotient,
            int maxPoints,
            int minPoints)
        {
            Number = number;
            Type = type;
            Name = name;
            Code = code;
            StartTime = startTime;
            Location = location;
            OwnerRaces = ownerRaces;
            PigeonRaces = pigeonRaces;

            RankPigeons(pointsQuotient, maxPoints, minPoints);
        }

        public Race(
            int number,
            RaceType type,
            string name,
            string code,
            DateTime startTime,
            Coordinate location,
            IList<OwnerRace> ownerRaces,
            IList<PigeonRace> pigeonRaces)
        {
            Number = number;
            Type = type;
            Name = name;
            Code = code;
            StartTime = startTime;
            Location = location;
            OwnerRaces = ownerRaces;
            PigeonRaces = pigeonRaces;
        }

        public int? Number { get; init; }

        public RaceType Type { get; init; }

        public string Name { get; init; }

        public string Code { get; init; }

        public DateTime StartTime { get; init; }

        public Coordinate Location { get; init; }

        public IList<OwnerRace> OwnerRaces { get; init; }

        public IList<PigeonRace> PigeonRaces { get; init; }

        /// <summary>
        /// Assigns points and position according to speed.
        /// </summary>
        /// <param name="pointsQuotient">The quotient for how many pigeons will store points, e.g. '3' will mean 1 in 3 pigeons scores points.</param>
        /// <param name="maxPoints">Amount of points for the first ranked pigeon.</param>
        /// <param name="minPoints">Amount of points for the last pigeon still scoring points according to <see href="pointsQuotient"/>.</param>
        public void RankPigeons(int pointsQuotient, double maxPoints, double minPoints)
        {
            double prizeCount = Math.Ceiling(Convert.ToDouble(PigeonRaces.Count) / pointsQuotient);
            double pointStep = (maxPoints - minPoints) / Math.Max(prizeCount - 1, 1);

            Dictionary<OwnerId, PigeonRace> lastPigeonByOwner = [];

            int position = 0;
            foreach (PigeonRace pigeonRace in PigeonRaces)
            {
                pigeonRace.Points = maxPoints - pointStep * position;
                pigeonRace.Position = ++position;

                PigeonRace? previousPigeonForOwner = lastPigeonByOwner.GetValueOrDefault(pigeonRace.OwnerId);
                if (previousPigeonForOwner is not null)
                    previousPigeonForOwner.Next = pigeonRace.Position;
                lastPigeonByOwner[pigeonRace.OwnerId] = pigeonRace;
            }
        }

        public override string ToString()
        {
            return $"{Code} {Name}";
        }
    }
}
