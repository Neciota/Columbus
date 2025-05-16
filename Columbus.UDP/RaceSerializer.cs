using Columbus.Models.Race;
using Columbus.UDP.Interfaces;

namespace Columbus.UDP
{
    public class RaceSerializer() : BaseSerializer, IRaceSerializer
    {

        public async Task<Race> DeserializeAsync(StreamReader stream)
        {
            UdpFile udpFile = await GetUdpAsync(stream);

            return udpFile.GetRace();
        }

        public Task<byte[]> SerializeAsync(Race race)
        {
            throw new NotImplementedException();
        }
    }
}