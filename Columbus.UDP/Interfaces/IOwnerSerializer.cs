using Columbus.Models.Owner;

namespace Columbus.UDP.Interfaces
{
    public interface IOwnerSerializer
    {
        Task<IEnumerable<Owner>> DeserializeAsync(StreamReader stream);
        Task<byte[]> SerializeAsync(IEnumerable<Owner> owners);
    }
}
