
namespace Columbus.Models.Race
{
    public class NoNeutralization : INeutralizationTime
    {
        public TimeSpan GetNeutralizedTime(DateTime rawArrivalTime, DateTime startTime)
        {
            return rawArrivalTime - startTime;
        }

        public bool IsInNeutralized(DateTime arrivalTime) => false;
    }
}
