using Columbus.Models;

namespace Columbus.UDP.Interfaces
{
    public interface IOwnerReader
    {
        IEnumerable<Owner> GetOwners();
    }
}
