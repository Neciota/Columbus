using Columbus.UDP.Lines;

namespace Columbus.UDP
{
    public abstract class BaseSerializer
    {
        internal static async Task<UdpFile> GetUdpAsync(StreamReader stream)
        {
            UdpFile udpFile = new();

            string? line;
            while ((line = await stream.ReadLineAsync()) is not null)
            {
                IUdpLine udpLine = Enum.Parse<LineType>(line.AsSpan(IUdpLine.TypeStart, IUdpLine.TypeLength)) switch
                {
                    LineType.Header => new HeaderLine(),
                    LineType.Owner => new OwnerLine(),
                    LineType.LevelEntry => new LevelEntryLine(),
                    LineType.ClockDeviation => new ClockDeviationLine(),
                    LineType.Pigeon => new PigeonLine(),
                    LineType.Footer => new FooterLine(),
                    _ => throw new NotImplementedException("Type not implemented.")
                };

                udpLine.Deserialize(line);
                udpFile.AddLine(udpLine);
            }

            return udpFile;
        }
    }
}
