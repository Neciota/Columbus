using Columbus.Models;
using Columbus.UDP.Interfaces;

namespace Columbus.UDP
{
    public class RaceReader : IOwnerReader, IPigeonReader, IRaceReader
    {
        private readonly string[] _allLines;
        private readonly IPigeonReader _pigeonReader;
        private readonly IOwnerReader _ownerReader;

        public RaceReader(string[] udp)
        {
            _pigeonReader = new PigeonReader(udp);
            _ownerReader = new OwnerReader(udp);
            _allLines = udp;
        }

        private string? RaceInfo { get; set; }

        private string? RaceCode { get; set; }

        private DateTime? RaceStart { get; set; }

        private Coordinate? RaceCoordinate { get; set; }

        private IEnumerable<Pigeon>? Pigeons { get; set; }

        private IEnumerable<Owner>? Owners { get; set; }

        private IEnumerable<OwnerRace>? OwnerRaces { get; set; }

        private IEnumerable<PigeonRace>? PigeonRaces { get; set; }

        public Race GetRace()
        {
            RaceInfo = GetRaceInfo();
            RaceCode = GetRaceCode();
            RaceStart = GetRaceStart();
            RaceCoordinate = GetRaceCoordinate();

            Owners = GetOwners();
            Pigeons = GetPigeons(Owners);

            PigeonRaces = GetPigeonRaces(Pigeons);
            OwnerRaces = GetOwnerRaces(Owners);

            return new Race(RaceInfo, RaceCode, RaceStart.Value, RaceCoordinate, OwnerRaces, PigeonRaces);
        }

        private string GetRaceInfo()
        {
            string id = _allLines[0].Substring(17, 2).Trim();
            string name = _allLines[0].Substring(19, 40).Trim();

            return $"{id} {name}";
        }

        private string GetRaceCode()
        {
            return _allLines[0].Substring(93, 8).Trim();
        }

        private DateTime GetRaceStart()
        {
            int year = Convert.ToInt32($"{HundredsOfYearsPrefix()}{_allLines[0].Substring(9, 2)}");
            int month = Convert.ToInt32(_allLines[0].Substring(7, 2));
            int day = Convert.ToInt32(_allLines[0].Substring(5, 2));
            int hour = Convert.ToInt32(_allLines[0].Substring(11, 2));
            int minute = Convert.ToInt32(_allLines[0].Substring(13, 2));
            int second = Convert.ToInt32(_allLines[0].Substring(15, 2));

            return new DateTime(year, month, day, hour, minute, second);
        }

        private Coordinate GetRaceCoordinate()
        {
            string xPos = _allLines[0].Substring(74, 9);
            string yPos = _allLines[0].Substring(84, 9);
            return new Coordinate(xPos, yPos);
        }

        private DateTime GetPigeonArrival(int i)
        {
            int year = Convert.ToInt32($"{HundredsOfYearsPrefix()}{_allLines[0].Substring(9, 2)}");
            int month = Convert.ToInt32(_allLines[i].Substring(54, 2));
            int day = Convert.ToInt32(_allLines[i].Substring(52, 2));
            int hour = Convert.ToInt32(_allLines[i].Substring(56, 2));
            int minute = Convert.ToInt32(_allLines[i].Substring(58, 2));
            int second = Convert.ToInt32(_allLines[i].Substring(60, 2));

            return new DateTime(year, month, day, hour, minute, second);
        }

        private IEnumerable<PigeonRace> GetPigeonRaces(IEnumerable<Pigeon> pigeons)
        {
            List<PigeonRace> pigeonRaces = new List<PigeonRace>();

            for (int i = 0; i < _allLines.Length; i++)
            {
                if (_allLines[i] == "" || _allLines[i].Substring(0, 2) != "40")
                    continue;

                int year = Convert.ToInt32(_allLines[i].Substring(20, 2));
                string country = _allLines[i].Substring(16, 2);
                int ringNumber = Convert.ToInt32(_allLines[i].Substring(25, 7));
                if (!pigeons.Any(p => p.Year == year && p.Country == country && p.RingNumber == ringNumber))
                    throw new Exception("This pigeon does not exist within the given list.");

                Pigeon pigeon = pigeons.First(p => p.Year == year && p.Country == country && p.RingNumber == ringNumber);

                int mark = Convert.ToInt32(_allLines[i].Substring(32, 3));

                DateTime arrivalTime;
                if (_allLines[i].Substring(48, 1) == "1")
                    arrivalTime = GetPigeonArrival(i);
                else
                    arrivalTime = new DateTime();

                pigeonRaces.Add(new PigeonRace(pigeon, arrivalTime, mark));
            }

            return pigeonRaces;
        }

        private List<OwnerRace> GetOwnerRaces(IEnumerable<Owner> owners)
        {
            return owners.Select(o => new OwnerRace(o, RaceCoordinate!, PigeonRaces!.Where(pr => pr.Pigeon.Owner.ID == o.ID).Count(), PigeonRaces!.Where(pr => pr.Pigeon.Owner.ID == o.ID).Sum(pr => pr.Points ?? 0)))
                .ToList();
        }

        private static string HundredsOfYearsPrefix() => (DateTime.Today.Year / 100).ToString();

        public IEnumerable<Owner> GetOwners()
        {
            return _ownerReader.GetOwners();
        }

        public IEnumerable<Pigeon> GetPigeons(IEnumerable<Owner> owners)
        {
            return _pigeonReader.GetPigeons(owners);
        }
    }
}