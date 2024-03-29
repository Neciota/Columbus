﻿using Columbus.Models;
using Columbus.UDP.Interfaces;

namespace Columbus.UDP
{
    public class RaceReader : IRaceReader
    {
        private readonly PigeonReader _pigeonReader;
        private readonly OwnerReader _ownerReader;

        public RaceReader()
        {
            _pigeonReader = new PigeonReader();
            _ownerReader = new OwnerReader();
        }

        private int RaceNumber { get; set; }

        private RaceType RaceType { get; set; }

        private string? RaceName { get; set; }

        private string? RaceCode { get; set; }

        private DateTime? RaceStart { get; set; }

        private Coordinate? RaceCoordinate { get; set; }

        private Dictionary<int, int>? OwnerClockDeviation { get; set; }

        private IList<OwnerRace>? OwnerRaces { get; set; }

        private IList<PigeonRace>? PigeonRaces { get; set; }

        public Race GetRace(StreamReader stream)
        {
            OwnerClockDeviation = new Dictionary<int, int>();
            PigeonRaces = new List<PigeonRace>();
            List<Owner> owners = new List<Owner>();

            while (!stream.EndOfStream)
            {
                string line = stream.ReadLine()!;
                string type = line.Substring(0, 2);
                switch (type)
                {
                    case "00": GetRaceInfo(line); break; // Race information opening line.
                    case "10": owners.Add(_ownerReader.GetOwner(line)); break; // Owner entry line.
                    case "20": continue; // Level entries.
                    case "30": OwnerClockDeviation[GetClockOwnerId(line)] = GetClockDeviation(line); break; // Synchronization markers with the atomic clock.
                    case "40": PigeonRaces.Add(GetPigeonRace(line, owners)); break; // Pigeon entry line.
                    case "50":
                    case "60":
                    case "70":
                    case "80": throw new NotImplementedException($"Type {type} not implemented."); // Does not seem to occur.
                    case "90": continue; // Presumably verification and information terminator line.
                }
            }

            OwnerRaces = GetOwnerRaces(owners);

            return new Race(RaceNumber, RaceType, RaceName!, RaceCode!, RaceStart!.Value, RaceCoordinate!, OwnerRaces!, PigeonRaces!);
        }

        public async Task<Race> GetRaceAsync(StreamReader stream)
        {
            OwnerClockDeviation = new Dictionary<int, int>();
            PigeonRaces = new List<PigeonRace>();
            List<Owner> owners = new List<Owner>();

            string line;
            while ((line = (await stream.ReadLineAsync())!) is not null)
            {
                string type = line.Substring(0, 2);
                switch (type)
                {
                    case "00": GetRaceInfo(line); break; // Race information opening line.
                    case "10": owners.Add(_ownerReader.GetOwner(line)); break; // Owner entry line.
                    case "20": continue; // Level entries.
                    case "30": OwnerClockDeviation[GetClockOwnerId(line)] = GetClockDeviation(line); break; // Synchronization markers with the atomic clock.
                    case "40": PigeonRaces.Add(GetPigeonRace(line, owners)); break; // Pigeon entry line.
                    case "50":
                    case "60":
                    case "70":
                    case "80": throw new NotImplementedException($"Type {type} not implemented."); // Does not seem to occur.
                    case "90": continue; // Presumably verification and information terminator line.
                }
            }

            OwnerRaces = GetOwnerRaces(owners);

            Race race = new Race(RaceNumber, RaceType, RaceName!, RaceCode!, RaceStart!.Value, RaceCoordinate!, OwnerRaces!, PigeonRaces!);

            return race;
        }

        private void GetRaceInfo(string line)
        {
            RaceNumber = GetRaceNumber(line);
            RaceType = GetRaceType(line);
            RaceName = GetRaceName(line);
            RaceCode = GetRaceCode(line);
            RaceStart = GetRaceStart(line);
            RaceCoordinate = GetRaceCoordinate(line);
        }

        private string GetRaceName(string line)
        {
            return line.Substring(19, 40).Trim();
        }

        private int GetRaceNumber(string line)
        {
            return Convert.ToInt32(line.Substring(17, 2).Trim());
        }

        private RaceType GetRaceType(string line)
        {
            string type = line.Substring(94, 1).Trim();

            switch (type)
            {
                case "T": { return RaceType.T; }
                case "V": { return RaceType.V; }
                case "M": { return RaceType.M; }
                case "E": { return RaceType.E; }
                case "O": { return RaceType.O; }
                case "Z": { return RaceType.Z; }
                case "J": { return RaceType.J; }
                case "L": { return RaceType.L; }
                case "X": { return RaceType.X; }
                case "N": { return RaceType.N; }
                case "F": { return RaceType.F; }
                default: { throw new ArgumentException($"No race type found by character '{type}'."); }
            }
        }

        private string GetRaceCode(string line)
        {
            return line.Substring(93, 8).Trim();
        }

        private DateTime GetRaceStart(string line)
        {
            int year = Convert.ToInt32($"{HundredsOfYearsPrefix()}{line.Substring(9, 2)}");
            int month = Convert.ToInt32(line.Substring(7, 2));
            int day = Convert.ToInt32(line.Substring(5, 2));
            int hour = Convert.ToInt32(line.Substring(11, 2));
            int minute = Convert.ToInt32(line.Substring(13, 2));
            int second = Convert.ToInt32(line.Substring(15, 2));

            return new DateTime(year, month, day, hour, minute, second);
        }

        private Coordinate GetRaceCoordinate(string line)
        {
            string xPos = line.Substring(74, 9);
            string yPos = line.Substring(84, 9);
            return new Coordinate(xPos, yPos);
        }

        private int GetClockOwnerId(string line)
        {
            return Convert.ToInt32(line.Substring(7, 8));
        }

        private int GetClockDeviation(string line)
        {
            DateTime atomicStopTime = GetDateTimeFromRange(line, RaceStart!.Value.Year, 37);
            DateTime clockStopTime = GetDateTimeFromRange(line, RaceStart!.Value.Year, 47);

            double deviation = (atomicStopTime - clockStopTime).TotalSeconds;
            return Convert.ToInt32(deviation);
        }

        private DateTime GetPigeonArrival(string line, int year)
        {
            return GetDateTimeFromRange(line, year, 52);
        }

        private PigeonRace GetPigeonRace(string line, IEnumerable<Owner> owners)
        {
            (Pigeon pigeon, int ownerId) = _pigeonReader.GetPigeonOwnerPair(line);
            Owner owner = owners.First(o => o.ID == ownerId);
            owner.Pigeons.Add(pigeon);

            int mark = Convert.ToInt32(line.Substring(32, 3));

            DateTime arrivalTime;
            if (line.Substring(48, 1) == "1")
            {
                arrivalTime = GetPigeonArrival(line, RaceStart!.Value.Year);
                arrivalTime = arrivalTime.AddSeconds(OwnerClockDeviation![owner.ID]);
            }
            else
                arrivalTime = new DateTime();

            return new PigeonRace(pigeon, arrivalTime, mark);
        }

        private IList<OwnerRace> GetOwnerRaces(IEnumerable<Owner> owners)
        {
            List<OwnerRace> ownerRaces = new List<OwnerRace>();

            foreach (var owner in owners)
            {
                HashSet<Pigeon> pigeons = owner.Pigeons.ToHashSet();
                int points = 0;
                int count = 0;

                foreach (PigeonRace pr in PigeonRaces!)
                {
                    if (!pigeons.Contains(pr.Pigeon))
                        continue;

                    points += pr.Points ?? 0;
                    count++;
                }

                ownerRaces.Add(new OwnerRace(owner, RaceCoordinate!, count, points));
            }

            return ownerRaces;
        }

        private static string HundredsOfYearsPrefix() => (DateTime.Today.Year / 100).ToString();

        private DateTime GetDateTimeFromRange(string line, int year, int startPos)
        {
            int month = Convert.ToInt32(line.Substring(startPos + 2, 2));
            int day = Convert.ToInt32(line.Substring(startPos, 2));
            int hour = Convert.ToInt32(line.Substring(startPos + 4, 2));
            int minute = Convert.ToInt32(line.Substring(startPos + 6, 2));
            int second = Convert.ToInt32(line.Substring(startPos + 8, 2));

            return new DateTime(year, month, day, hour, minute, second);
        }
    }
}