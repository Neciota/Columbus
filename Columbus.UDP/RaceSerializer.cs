using Columbus.Models.Race;
using Columbus.UDP.Interfaces;
using Columbus.UDP.Lines;
using Columbus.UDP.Lines.Race;
using Columbus.UDP.UdpFiles;

namespace Columbus.UDP
{
    public class RaceSerializer : BaseSerializer, IRaceSerializer
    {

        public async Task<Race> DeserializeAsync(StreamReader stream)
        {
            RaceUdpFile udpFile = await GetUdpAsync<RaceUdpFile>(stream, UdpType.Race);

            return udpFile.GetRace();
        }

        public Task<byte[]> SerializeAsync(Race race)
        {
            throw new NotImplementedException();
        }

        internal override IUdpLine GetLine(string line)
        {
            return Enum.Parse<LineType>(line.AsSpan(IUdpLine.TypeStart, IUdpLine.TypeLength)) switch
            {
                LineType.Header => new HeaderLine(),
                LineType.Owner => new OwnerLine(),
                LineType.LevelEntry => new LevelEntryLine(),
                LineType.ClockDeviation => new ClockDeviationLine(),
                LineType.Pigeon => new PigeonLine(),
                LineType.Footer => new FooterLine(),
                _ => throw new NotImplementedException("Type not implemented.")
            };
        }
    }
}