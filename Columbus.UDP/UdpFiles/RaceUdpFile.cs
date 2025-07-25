using Columbus.Models.Owner;
using Columbus.Models.Pigeon;
using Columbus.Models.Race;
using Columbus.UDP.Lines;
using Columbus.UDP.Lines.Race;

namespace Columbus.UDP.UdpFiles
{
    internal class RaceUdpFile : IUdpFile
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

        public Race GetRace(INeutralizationTime neutralizationTime)
        {
            Dictionary<OwnerId, OwnerRace> ownerRaces = Owners.Select(GetOwnerRaceFromOwner)
                .ToDictionary(or => or.Owner.Id);
            List<PigeonRace> pigeonRaces = Pigeons.Select(GetPigeonRaceFromPigeon)
                .OrderByDescending(pr => pr.GetSpeed(ownerRaces[pr.OwnerId].Distance, Header.RaceStart, ownerRaces[pr.OwnerId].ClockDeviation, neutralizationTime))
                .ToList();

            return new Race(
                Header.Number,
                RaceType.Parse(Header.FileName.AsSpan(1, 1)),
                Header.Name,
                Header.FileName.Substring(0, 4),
                Header.RaceStart,
                Header.Location,
                ownerRaces.Values.ToList(),
                pigeonRaces);
        }

        private OwnerRace GetOwnerRaceFromOwner(OwnerLine ownerLine)
        {
            Owner owner = new(ownerLine.Id, ownerLine.Name, ownerLine.LoftLocation, ownerLine.Club);
            LevelEntryLine? entries = LevelEntries.Where(le => le.OwnerId == ownerLine.Id).MinBy(le => le.Level);
            ClockDeviationLine? deviationLine = ClockDeviations.FirstOrDefault(cd => cd.OwnerId == ownerLine.Id);
            TimeSpan clockDeviation = deviationLine is null ? TimeSpan.Zero : deviationLine.SubmissionAtomicClockTime - deviationLine.SubmissionOwnerClockTime;

            return new(owner, owner.LoftCoordinate.GetDistance(Header.Location), entries?.Count ?? 0, clockDeviation);
        }

        private PigeonRace GetPigeonRaceFromPigeon(PigeonLine pigeonLine)
        {
            Pigeon pigeon = new(pigeonLine.Country, pigeonLine.Year, pigeonLine.RingNumber, pigeonLine.Chip, pigeonLine.Sex);

            // Because the UDP does not store the year for each individual serialized date/time, the serializer reads them into current year automatically.
            // We assume the actual year is always the race's year and correct accordingly.
            DateTime? arrival;
            if (pigeonLine.Arrival is not null)
                arrival = pigeonLine.Arrival.Value.AddYears(Header.RaceStart.Year - pigeonLine.Arrival.Value.Year);
            else
                arrival = pigeonLine.Arrival;

            return new(pigeon, pigeonLine.OwnerId, arrival, pigeonLine.Mark);
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
