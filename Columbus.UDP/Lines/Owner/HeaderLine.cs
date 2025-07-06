using Columbus.UDP.UdpFiles;
using System.Globalization;

namespace Columbus.UDP.Lines.Owner
{
    internal class HeaderLine : IUdpLine
    {
        private const int UdpTypeStart = 4;
        private const int UdpTypeLength = 1;
        private const int HashStart = 97;
        private const int HashLength = 8;

        public LineType Type { get; } = LineType.Header;
        public UdpType UdpType { get; set; }
        public int Hash { get; private set; }

        public void Deserialize(string line)
        {
            UdpType = Enum.Parse<UdpType>(line.AsSpan(UdpTypeStart, UdpTypeLength));
            Hash = int.Parse(line.AsSpan(HashStart, HashLength), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
