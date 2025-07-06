using Columbus.Models.Owner;
using Columbus.Models.Pigeon;
using Columbus.UDP.Interfaces;
using Columbus.UDP.Lines;
using Columbus.UDP.UdpFiles;

namespace Columbus.UDP
{
    public class PigeonSerializer : BaseSerializer, IPigeonSerializer
    {
        public async Task<IEnumerable<Pigeon>> DeserializeAsync(StreamReader stream)
        {
            PigeonUdpFile udpFile = await GetUdpAsync<PigeonUdpFile>(stream, UdpType.Pigeons);

            return udpFile.GetPigeons();
        }

        public Task<byte[]> SerializeAsync(IEnumerable<Owner> owners, IEnumerable<Pigeon> pigeons)
        {
            throw new NotImplementedException();
        }

        internal override IUdpLine GetLine(string line)
        {
            throw new NotImplementedException();
        }
    }
}
