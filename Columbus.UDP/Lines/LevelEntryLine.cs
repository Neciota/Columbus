using Columbus.Models.Owner;
using Columbus.Models;
using System.Globalization;

namespace Columbus.UDP.Lines
{
    internal class LevelEntryLine : IUdpLine
    {
        private const int ClubStart = 3;
        private const int ClubLength = 4;
        private const int OwnerStart = 7;
        private const int OwnerLength = 8;
        private const int LevelStart = 15;
        private const int LevelLength = 2;
        private const int CountStart = 17;
        private const int CountLength = 5;
        private const int HashStart = 128;
        private const int HashLength = 8;

        public LineType Type { get; } = LineType.LevelEntry;
        public ClubId ClubId { get; private set; }
        public OwnerId OwnerId { get; private set; }
        public int Level { get; private set; }
        public int Count { get; private set; }
        public int Hash { get; private set; }

        public void Deserialize(string line)
        {
            ClubId = ClubId.Parse(line.AsSpan(ClubStart, ClubLength), CultureInfo.InvariantCulture);
            OwnerId = OwnerId.Parse(line.AsSpan(OwnerStart, OwnerLength), CultureInfo.InvariantCulture);
            Level = int.Parse(line.AsSpan(LevelStart, LevelLength), CultureInfo.InvariantCulture);
            Count = int.Parse(line.AsSpan(CountStart, CountLength), CultureInfo.InvariantCulture);
            Hash = int.Parse(line.AsSpan(HashStart, HashLength), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
