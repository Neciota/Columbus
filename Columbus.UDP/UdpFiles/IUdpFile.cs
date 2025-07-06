using Columbus.UDP.Lines;

namespace Columbus.UDP.UdpFiles
{
    internal interface IUdpFile
    {
        void AddLine(IUdpLine udpLine);
    }
}
