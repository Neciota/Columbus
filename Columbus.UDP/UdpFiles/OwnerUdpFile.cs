using Columbus.Models.Owner;
using Columbus.Models.Pigeon;
using Columbus.UDP.Lines;
using Columbus.UDP.Lines.Owner;
using System.Linq;

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
            ILookup<OwnerId, Pigeon> pigeonsByOwnerId = Pigeons.Select(GetPigeonByOwnerFromLine).ToLookup(po => po.OwnerId, po => po.Pigeon);

            Owner[] owners = Owners.Select(o => GetOwnerFromLine(o, pigeonsByOwnerId)).ToArray();

            return owners;
        }

        private static Owner GetOwnerFromLine(OwnerLine ownerLine, ILookup<OwnerId, Pigeon> pigeonsByOwnerId)
        {
            return new(ownerLine.Id, ownerLine.Name, ownerLine.LoftLocation, ownerLine.Club, pigeonsByOwnerId[ownerLine.Id].ToList());
        }

        private static (OwnerId OwnerId, Pigeon Pigeon) GetPigeonByOwnerFromLine(PigeonLine pigeonLine)
        {
            return (pigeonLine.OwnerId, new(pigeonLine.Country, pigeonLine.Year, pigeonLine.RingNumber, pigeonLine.Chip, pigeonLine.Sex));
        }
    }
}
