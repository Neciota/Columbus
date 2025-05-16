using System.Globalization;

namespace Columbus.UDP.Lines
{
    internal class FooterLine : IUdpLine
    {
        private const int HashStart = 166;
        private const int HashLength = 8;

        public LineType Type { get; } = LineType.Footer;
        public int Hash { get; private set; }

        public void Deserialize(string line)
        {
            Hash = int.Parse(line.AsSpan(HashStart, HashLength), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
