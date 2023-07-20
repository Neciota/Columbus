using Columbus.Models;

namespace Columbus.UDP.Interfaces
{
    public interface IPigeonReader
    {
        IEnumerable<Pigeon> GetPigeons(IEnumerable<Owner> owners);
    }
}
