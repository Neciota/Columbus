using Columbus.Models;

namespace Columbus.UDP.Interfaces
{
    public interface IOwnerReader
    {
        IEnumerable<Owner> GetOwners(StreamReader stream);
        Task<IEnumerable<Owner>> GetOwnersAsync(StreamReader stream);
    }
}
