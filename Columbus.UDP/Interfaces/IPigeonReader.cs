using Columbus.Models;

namespace Columbus.UDP.Interfaces
{
    public interface IPigeonReader
    {
        IEnumerable<Pigeon> GetPigeons(StreamReader stream);
        Task<IEnumerable<Pigeon>> GetPigeonsAsync(StreamReader stream);
    }
}
