using Columbus.Models.Owner;
using Columbus.Models.Pigeon;
using Columbus.Models.Race;
using Columbus.UDP.Lines;

namespace Columbus.UDP
{
    internal class UdpFile
    {
        public HeaderLine Header { get; private set; } = new();
        public List<OwnerLine> Owners { get; private set; } = [];
        public List<LevelEntryLine> LevelEntries { get; private set; } = [];
        public List<ClockDeviationLine> ClockDeviations { get; private set; } = [];
        public List<PigeonLine> Pigeons { get; private set; } = [];
        public FooterLine Footer { get; private set; } = new();

        public void AddLine(IUdpLine udpLine)
        {
            switch (udpLine)
            {
                case HeaderLine header: 
                    Header = header;
                    break;
                case OwnerLine owner:
                    Owners.Add(owner);
                    break;
                case LevelEntryLine levelEntry:
                    LevelEntries.Add(levelEntry);
                    break;
                case ClockDeviationLine clockDeviation:
                    ClockDeviations.Add(clockDeviation);
                    break;
                case PigeonLine pigeon:
                    Pigeons.Add(pigeon);
                    break;
                case FooterLine footer:
                    Footer = footer;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public Race GetRace()
        {
            Dictionary<OwnerId, OwnerRace> ownerRaces = Owners.Select(GetOwnerRaceFromOwner)
                .ToDictionary(or => or.Owner.Id);
            List<PigeonRace> pigeonRaces = Pigeons.Select(GetPigeonRaceFromPigeon)
                .OrderByDescending(pr => pr.GetSpeed(ownerRaces[pr.OwnerId].Distance, Header.RaceStart, ownerRaces[pr.OwnerId].ClockDeviation))
                .ToList();

            return new Race(
                Header.Number,
                RaceType.T,
                Header.Name,
                string.Empty,
                Header.RaceStart,
                Header.Location,
                ownerRaces.Values.ToList(),
                pigeonRaces,
                3,
                500,
                30);
        }

        private OwnerRace GetOwnerRaceFromOwner(OwnerLine ownerLine)
        {
            Owner owner = new(ownerLine.Id, ownerLine.Name, ownerLine.LoftLocation, ownerLine.Club);
            LevelEntryLine? entries = LevelEntries.Where(le => le.OwnerId == ownerLine.Id).MinBy(le => le.Level);
            ClockDeviationLine deviationLine = ClockDeviations.First(cd => cd.OwnerId == ownerLine.Id);
            TimeSpan clockDeviation = deviationLine.SubmissionAtomicClockTime - deviationLine.SubmissionOwnerClockTime;

            return new(owner, owner.LoftCoordinate.GetDistance(Header.Location), entries?.Count ?? 0, clockDeviation);
        }

        private static PigeonRace GetPigeonRaceFromPigeon(PigeonLine pigeonLine)
        {
            Pigeon pigeon = new(pigeonLine.Country, pigeonLine.Year, pigeonLine.RingNumber, pigeonLine.Chip, pigeonLine.Sex);

            return new(pigeon, pigeonLine.OwnerId, pigeonLine.Arrival, pigeonLine.Mark);
        }

        public IEnumerable<Owner> GetOwners()
        {
            return Owners.Select(GetOwnerFromLine).ToArray();
        }

        private static Owner GetOwnerFromLine(OwnerLine ownerLine)
        {
            return new(ownerLine.Id, ownerLine.Name, ownerLine.LoftLocation, ownerLine.Club);
        }

        public IEnumerable<Pigeon> GetPigeons()
        {
            return Pigeons.Select(GetPigeonFromLine).ToArray();
        }

        private static Pigeon GetPigeonFromLine(PigeonLine pigeonLine)
        {
            return new(pigeonLine.Country, pigeonLine.Year, pigeonLine.RingNumber, pigeonLine.Chip, pigeonLine.Sex);
        }
    }
}
