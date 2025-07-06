using Columbus.Models.Owner;
using Columbus.UDP.Lines;
using Columbus.UDP.Lines.Owner;

namespace Columbus.UDP.UdpFiles
{
    internal class OwnerUdpFile : IUdpFile
    {
        public HeaderLine Header { get; private set; } = new();
        public List<OwnerLine> Owners { get; private set; } = [];
        public List<PigeonLine> Pigeons { get; private set; } = [];
        public FooterLine Footer { get; private set; } = new();

        public void AddLine(IUdpLine udpLine)
        {
            switch (udpLine)
            {
                case HeaderLine header:
                    Header = header;
                    break;
                case OwnerLine owner:
                    Owners.Add(owner);
                    break;
                case PigeonLine pigeon:
                    Pigeons.Add(pigeon);
                    break;
                case FooterLine footer:
                    Footer = footer;
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public IEnumerable<Owner> GetOwners()
        {
            return Owners.Select(GetOwnerFromLine).ToArray();
        }

        private static Owner GetOwnerFromLine(OwnerLine ownerLine)
        {
            return new(ownerLine.Id, ownerLine.Name, ownerLine.LoftLocation, ownerLine.Club);
        }
    }
}
