using Columbus.Models.Owner;
using Columbus.Models.Pigeon;
using Columbus.UDP.Interfaces;

namespace Columbus.UDP
{
    public class PigeonSerializer : BaseSerializer, IPigeonSerializer
    {
        public async Task<IEnumerable<Pigeon>> DeserializeAsync(StreamReader stream)
        {
            UdpFile udpFile = await GetUdpAsync(stream);

            return udpFile.GetPigeons();
        }

        public Task<byte[]> SerializeAsync(IEnumerable<Owner> owners, IEnumerable<Pigeon> pigeons)
        {
            throw new NotImplementedException();
        }
    }
}
