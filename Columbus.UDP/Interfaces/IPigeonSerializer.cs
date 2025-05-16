using Columbus.Models.Owner;
using Columbus.Models.Pigeon;

namespace Columbus.UDP.Interfaces
{
    public interface IPigeonSerializer
    {
        Task<IEnumerable<Pigeon>> DeserializeAsync(StreamReader stream);
        Task<byte[]> SerializeAsync(IEnumerable<Owner> owners, IEnumerable<Pigeon> pigeons);
    }
}
