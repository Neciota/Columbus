namespace Columbus.Models
{
    public class Race
    {
        public Race(int number, RaceType type, string name, string code, DateTime startTime, Coordinate location, IList<OwnerRace> ownerRaces, IList<PigeonRace> pigeonRaces)
        {
            Number = number;
            Type = type;
            Name = name;
            Code = code;
            StartTime = startTime;
            Location = location;
            OwnerRaces = ownerRaces;
            PigeonRaces = pigeonRaces;

            CalculateResults();
        }

        public Race(RaceType type, string name, string code, DateTime startTime, Coordinate location, IList<OwnerRace> ownerRaces, IList<PigeonRace> pigeonRaces)
        {
            Type = type;
            Name = name;
            Code = code;
            StartTime = startTime;
            Location = location;
            OwnerRaces = ownerRaces;
            PigeonRaces = pigeonRaces;

            CalculateResults();
        }

        public int? Number { get; set; }

        public RaceType Type { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public DateTime StartTime { get; set; }

        public Coordinate Location { get; set; }

        public IList<OwnerRace> OwnerRaces { get; set; }

        public IList<PigeonRace> PigeonRaces { get; set; }

        public void CalculateResults()
        {
            foreach (OwnerRace ownerRace in OwnerRaces)
            {
                HashSet<Pigeon> ownerPigeons = ownerRace.Owner.Pigeons.ToHashSet();

                foreach (PigeonRace pigeonRace in PigeonRaces.Where(pr => ownerPigeons.Contains(pr.Pigeon)))
                    pigeonRace.CalculateSpeed(ownerRace.Distance, StartTime);
            }

            RankPigeons();
            AssignNextPigeon();
        }

        private void RankPigeons()
        {
            PigeonRaces = PigeonRaces.OrderByDescending(p => p.Speed).ToList();

            double prizeCount = Math.Ceiling(Convert.ToDouble(PigeonRaces.Count) / 3);
            double pointStep = 470.0 / Math.Max(prizeCount - 1, 1);
            for (int i = 0; i < prizeCount; i++)
            {
                PigeonRaces[i].Points = Convert.ToInt32(Math.Round(500.0 - pointStep * i));
            }

            for (int i = 0; i < PigeonRaces.Count; i++)
            {
                PigeonRaces[i].Position = i + 1;
            }
        }

        private void AssignNextPigeon()
        {
            foreach (OwnerRace ownerRace in OwnerRaces)
            {
                HashSet<Pigeon> ownerPigeons = ownerRace.Owner.Pigeons.ToHashSet();

                List<PigeonRace> ownerPigeonRaces = PigeonRaces.Where(pr => ownerPigeons.Contains(pr.Pigeon)).ToList();
                for (int i = 0; i < ownerPigeonRaces.Count - 1; i++)
                {
                    ownerPigeonRaces[i].Next = ownerPigeonRaces[i + 1].Position;
                }
            }
        }

        public override string ToString()
        {
            return $"{Code} {Name}";
        }
    }
}
