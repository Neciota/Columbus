using Columbus.Models;
using Columbus.UDP.Interfaces;

namespace Columbus.UDP
{
    public class OwnerReader : PigeonReader, IOwnerReader
    {
        private readonly string[] _allLines;

        public OwnerReader(string[] udp): base(udp)
        {
            _allLines = udp;
        }

        public IEnumerable<Owner> GetOwners()
        {
            List<Owner> owners = new List<Owner>();

            for (int i = 0; i < _allLines.Length; i++)
            {
                if (_allLines[i] == "" || _allLines[i].Substring(0, 3) != "107")
                    continue;
                int id = Convert.ToInt32(_allLines[i].Substring(7, 8));
                string name = _allLines[i].Substring(50, 20).Trim();

                string xPos = _allLines[i].Substring(30, 9).Trim();
                string yPos = _allLines[i].Substring(40, 9).Trim();
                Coordinate? coordinate = null;
                if (!string.IsNullOrEmpty(xPos) && !string.IsNullOrEmpty(yPos))
                    coordinate = new Coordinate(xPos, yPos);

                int club = Convert.ToInt32(_allLines[i].Substring(3, 4));

                Owner owner = new Owner(id, name, coordinate, club);
                owners.Add(owner);
            }

            GetPigeons(owners);

            return owners;
        }
    }
}
