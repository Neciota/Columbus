using Columbus.Models;

namespace Columbus.UDP.Interfaces
{
    public interface IRaceReader
    {
        Race GetRace(StreamReader stream);
    }
}
