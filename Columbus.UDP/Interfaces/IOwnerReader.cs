using Columbus.Models;

namespace Columbus.UDP.Interfaces
{
    public interface IOwnerReader : IPigeonReader
    {
        IEnumerable<Owner> GetOwners();
    }
}
