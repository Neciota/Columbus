using Columbus.Models;
using Columbus.UDP.Interfaces;

namespace Columbus.UDP
{
    public class PigeonReader: IPigeonReader
    {
        private readonly string[] _allLines;

        public PigeonReader(string[] udp)
        {
            _allLines = udp;
        }

        public IEnumerable<Pigeon> GetPigeons(IEnumerable<Owner> owners)
        {
            List<Pigeon> pigeons = new List<Pigeon>();

            for (int i = 0; i < _allLines.Length; i++)
            {
                if (_allLines[i] == "" || _allLines[i].Substring(0, 2) != "40")
                    continue;

                int year = Convert.ToInt32(_allLines[i].Substring(20, 2));
                string country = _allLines[i].Substring(16, 2);
                int ringNumber = Convert.ToInt32(_allLines[i].Substring(25, 7));

                int ownerId = Convert.ToInt32(_allLines[i].Substring(7, 8));
                Owner owner = owners.First(o => o.ID == ownerId);

                int chip;
                if (!int.TryParse(_allLines[i].Substring(109, 7), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out chip))
                {
                    chip = 0;
                }

                Sex sex = GetPigeonSex(_allLines[i].Substring(15, 1));

                pigeons.Add(new Pigeon(country, year, ringNumber, owner, chip, sex));
            }

            return pigeons;
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
