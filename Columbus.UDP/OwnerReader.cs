using Columbus.Models;
using Columbus.UDP.Interfaces;
using System.Runtime.CompilerServices;

namespace Columbus.UDP
{
    public class OwnerReader : IOwnerReader
    {
        private PigeonReader _pigeonReader;

        public OwnerReader()
        {
            _pigeonReader = new PigeonReader();
        }

        public IEnumerable<Owner> GetOwners(StreamReader stream)
        {
            List<Owner> owners = new List<Owner>();

            while (!stream.EndOfStream)
            {
                string line = stream.ReadLine()!;
                string type = line.Substring(0, 2);
                switch (type)
                {
                    case "00": continue; // Occurs, not needed here.
                    case "10": owners.Add(GetOwner(line)); break; // Owner entry line.
                    case "20": continue; // Occurs, but unknown use.
                    case "30": continue; // Occurs, but unknown use.
                    case "40":
                        (Pigeon pigeon, int ownerId) = _pigeonReader.GetPigeonOwnerPair(line);
                        Owner owner = owners.First(o => o.ID == ownerId);
                        owner.Pigeons.Add(pigeon);
                        break;
                    case "50":
                    case "60":
                    case "70":
                    case "80": throw new NotImplementedException($"Type {type} not implemented."); // Does not seem to occur.
                    case "90": continue; // Presumably verification and information terminator line.
                }
            }

            return owners;
        }

        public async Task<IEnumerable<Owner>> GetOwnersAsync(StreamReader stream)
        {
            List<Owner> owners = new List<Owner>();

            string line;
            while ((line = (await stream.ReadLineAsync())!) is not null)
            {
                string type = line.Substring(0, 2);
                switch (type)
                {
                    case "00": continue; // Occurs, not needed here.
                    case "10": owners.Add(GetOwner(line)); break; // Owner entry line.
                    case "20": continue; // Occurs, but unknown use.
                    case "30": continue; // Occurs, but unknown use.
                    case "40":
                        (Pigeon pigeon, int ownerId) = _pigeonReader.GetPigeonOwnerPair(line);
                        Owner owner = owners.First(o => o.ID == ownerId);
                        owner.Pigeons.Add(pigeon);
                        break;
                    case "50":
                    case "60":
                    case "70":
                    case "80": throw new NotImplementedException($"Type {type} not implemented."); // Does not seem to occur.
                    case "90": continue; // Presumably verification and information terminator line.
                }
            }

            return owners;
        }

        internal Owner GetOwner(string line)
        {
            int id = Convert.ToInt32(line.Substring(7, 8));
            string name = line.Substring(50, 20).Trim();

            string xPos = line.Substring(30, 9).Trim();
            string yPos = line.Substring(40, 9).Trim();
            Coordinate? coordinate = null;
            if (!string.IsNullOrEmpty(xPos) && !string.IsNullOrEmpty(yPos))
                coordinate = new Coordinate(xPos, yPos);

            int club = Convert.ToInt32(line.Substring(3, 4));

            return new Owner(id, name, coordinate, club);
        }
    }
}
