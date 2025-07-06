using Columbus.UDP.Lines;
using Columbus.UDP.UdpFiles;

namespace Columbus.UDP
{
    public abstract class BaseSerializer
    {
        internal async Task<TUdpFile> GetUdpAsync<TUdpFile>(StreamReader stream, UdpType udpType) where TUdpFile : IUdpFile
        {
            IUdpFile udpFile = CreateUdp(udpType);

            string? line;
            while ((line = await stream.ReadLineAsync()) is not null)
            {
                IUdpLine udpLine = GetLine(line);

                udpLine.Deserialize(line);
                udpFile.AddLine(udpLine);
            }

            return (TUdpFile)udpFile;
        }

        private static IUdpFile CreateUdp(UdpType udpType) => udpType switch
        {
            UdpType.OwnersAndPigeons => new OwnerUdpFile(),
            UdpType.Race => new RaceUdpFile(),
            _ => throw new NotImplementedException($"No udp deserializer implemented for type {udpType}."),
        };

        internal abstract IUdpLine GetLine(string line);
    }
}
