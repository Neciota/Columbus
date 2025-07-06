using Columbus.Models;
using Columbus.UDP.UdpFiles;
using System.Globalization;

namespace Columbus.UDP.Lines.Race
{
    internal class HeaderLine : IUdpLine
    {
        private const int UdpTypeStart = 4;
        private const int UdpTypeLength = 1;
        private const int RaceStartStart = 5;
        private const int RaceStartLength = 12;
        private const int NumberStart = 16;
        private const int NumberLength = 3;
        private const int NameStart = 19;
        private const int NameLength = 20;
        private const int DescriptionStart = 39;
        private const int DescriptionLength = 20;
        private const int LocationStart = 73;
        private const int LocationLength = 20;
        private const int FileNameStart = 93;
        private const int FileNameLength = 12;
        private const int ClubStart = 108;
        private const int ClubLength = 4;
        private const int FileCreatedAtStart = 122;
        private const int FileCreatedAtLength = 12;
        private const int HashStart = 134;
        private const int HashLength = 8;

        public LineType Type { get; } = LineType.Header;
        public UdpType UdpType { get; set; }
        public int Number { get; private set; }
        public DateTime RaceStart { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public Coordinate Location { get; private set; }
        public string FileName { get; private set; } = string.Empty;
        public ClubId Club { get; private set; }
        public DateTime FileCreatedAt { get; private set; }
        public int Hash { get; private set; }

        public void Deserialize(string line)
        {
            UdpType = Enum.Parse<UdpType>(line.AsSpan(UdpTypeStart, UdpTypeLength));
            Number = int.Parse(line.AsSpan(NumberStart, NumberLength));
            Name = line.Substring(NameStart, NameLength).Trim();
            Description = line.Substring(DescriptionStart, DescriptionLength).Trim();
            FileName = line.Substring(FileNameStart, FileNameLength).Trim();
            RaceStart = DateTime.ParseExact(line.AsSpan(RaceStartStart, RaceStartLength), "ddMMyyHHmmss", CultureInfo.InvariantCulture);
            Location = Coordinate.ParseFromDms(line.AsSpan(LocationStart, LocationLength), CultureInfo.InvariantCulture);
            Club = ClubId.Parse(line.AsSpan(ClubStart, ClubLength), CultureInfo.InvariantCulture);
            FileCreatedAt = DateTime.ParseExact(line.AsSpan(FileCreatedAtStart, FileCreatedAtLength), "ddMMyyHHmmss", CultureInfo.InvariantCulture);
            Hash = int.Parse(line.AsSpan(HashStart, HashLength), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
