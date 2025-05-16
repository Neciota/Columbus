using Columbus.Models.Owner;
using Columbus.UDP.Interfaces;

namespace Columbus.UDP
{
    public class OwnerSerializer : BaseSerializer, IOwnerSerializer
    {
        public async Task<IEnumerable<Owner>> DeserializeAsync(StreamReader stream)
        {
            UdpFile udpFile = await GetUdpAsync(stream);

            return udpFile.GetOwners();
        }

        public Task<byte[]> SerializeAsync(IEnumerable<Owner> owners)
        {
            throw new NotImplementedException();
        }
    }
}
