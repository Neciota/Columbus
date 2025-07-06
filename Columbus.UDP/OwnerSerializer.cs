using Columbus.Models.Owner;
using Columbus.UDP.Interfaces;
using Columbus.UDP.Lines;
using Columbus.UDP.Lines.Owner;
using Columbus.UDP.UdpFiles;

namespace Columbus.UDP
{
    public class OwnerSerializer : BaseSerializer, IOwnerSerializer
    {
        public async Task<IEnumerable<Owner>> DeserializeAsync(StreamReader stream)
        {
            OwnerUdpFile udpFile = await GetUdpAsync<OwnerUdpFile>(stream, UdpType.OwnersAndPigeons);

            return udpFile.GetOwners();
        }

        public Task<byte[]> SerializeAsync(IEnumerable<Owner> owners)
        {
            throw new NotImplementedException();
        }

        internal override IUdpLine GetLine(string line)
        {
            return Enum.Parse<LineType>(line.AsSpan(IUdpLine.TypeStart, IUdpLine.TypeLength)) switch
            {
                LineType.Header => new HeaderLine(),
                LineType.Owner => new OwnerLine(),
                LineType.Pigeon => new PigeonLine(),
                LineType.Footer => new FooterLine(),
                _ => throw new NotImplementedException("Type not implemented.")
            };
        }
    }
}
