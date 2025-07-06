using Columbus.Models.Pigeon;
using Columbus.UDP.Lines;

namespace Columbus.UDP.UdpFiles
{
    internal class PigeonUdpFile : IUdpFile
    {
        public void AddLine(IUdpLine udpLine)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Pigeon> GetPigeons() => throw new NotImplementedException();
    }
}
