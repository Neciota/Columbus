using Columbus.Models.Race;

namespace Columbus.UDP.Interfaces
{
    public interface IRaceSerializer
    {
        Task<Race> DeserializeAsync(StreamReader stream);
        Task<byte[]> SerializeAsync(Race race);
    }
}
