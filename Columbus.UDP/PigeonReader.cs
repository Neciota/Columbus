using Columbus.Models;
using Columbus.UDP.Interfaces;

namespace Columbus.UDP
{
    public class PigeonReader: IPigeonReader
    {
        public PigeonReader() { }

        public IEnumerable<Pigeon> GetPigeons(StreamReader stream)
        {
            List<Pigeon> pigeons = new List<Pigeon>();

            while (!stream.EndOfStream)
            {
                string line = stream.ReadLine()!;
                string type = line.Substring(0, 2);
                switch (type)
                {
                    case "00": continue; // Occurs, not needed here.
                    case "10": continue; // Occurs, not needed here.
                    case "20": continue; // Occurs, but unknown use.
                    case "30": continue; // Occurs, but unknown use.
                    case "40":
                        (Pigeon pigeon, _) = GetPigeonOwnerPair(line);
                        pigeons.Add(pigeon);
                        break;
                    case "50":
                    case "60":
                    case "70":
                    case "80": throw new NotImplementedException($"Type {type} not implemented."); // Does not seem to occur.
                    case "90": continue; // Presumably verification and information terminator line.
                }
            }

            return pigeons;
        }

        public async Task<IEnumerable<Pigeon>> GetPigeonsAsync(StreamReader stream)
        {
            List<Pigeon> pigeons = new List<Pigeon>();

            while (!stream.EndOfStream)
            {
                string line = (await stream.ReadLineAsync())!;
                string type = line.Substring(0, 2);
                switch (type)
                {
                    case "00": continue; // Occurs, not needed here.
                    case "10": continue; // Occurs, not needed here.
                    case "20": continue; // Occurs, but unknown use.
                    case "30": continue; // Occurs, but unknown use.
                    case "40":
                        (Pigeon pigeon, _) = GetPigeonOwnerPair(line);
                        pigeons.Add(pigeon);
                        break;
                    case "50":
                    case "60":
                    case "70":
                    case "80": throw new NotImplementedException($"Type {type} not implemented."); // Does not seem to occur.
                    case "90": continue; // Presumably verification and information terminator line.
                }
            }

            return pigeons;
        }

        internal (Pigeon, int) GetPigeonOwnerPair(string line)
        {
            if (line == "" || line.Substring(0, 2) != "40")
                throw new ArgumentException("Line is not of pigeon type; must start with 40");

            int year = Convert.ToInt32(line.Substring(20, 2)) + 2000;
            string country = line.Substring(16, 2);
            int ringNumber = Convert.ToInt32(line.Substring(25, 7));

            int ownerId = Convert.ToInt32(line.Substring(7, 8));

            int chip;
            if (!int.TryParse(line.Substring(109, 7), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out chip))
            {
                chip = 0;
            }

            Sex sex = GetPigeonSex(line.Substring(15, 1));

            return (new Pigeon(country, year, ringNumber, chip, sex), ownerId);
        }

        private Sex GetPigeonSex(string indicator)
        {
            switch (indicator)
            {
                case "M": return Sex.MALE;
                case "V": return Sex.FEMALE;
                default: return Sex.UNKNOWN;
            }
        }
    }
}
