using Columbus.Models;

namespace Columbus.UDP.Interfaces
{
    public interface IRaceReader : IOwnerReader, IPigeonReader
    {
        Race GetRace();
    }
}
