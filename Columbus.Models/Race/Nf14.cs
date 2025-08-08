namespace Columbus.Models.Race
{
    /// <summary>
    /// Neutralization factor 2014
    /// </summary>
    /// <param name="neutralisationTimes"></param>
    public class Nf14(Dictionary<DateOnly, (DateTime SunUp, DateTime SunDown)> neutralisationTimes) : INeutralizationTime
    {
        // Time before/after sundown during which the race is not yet neutralized.
        private readonly TimeSpan _gracePeriod = TimeSpan.FromMinutes(30);
        private readonly Dictionary<DateOnly, (DateTime SunUp, DateTime SunDown)> _neutralisationTimes = neutralisationTimes;

        public (DateTime SunUp, DateTime SunDown) this[DateTime index] => _neutralisationTimes[DateOnly.FromDateTime(index)];
        public (DateTime SunUp, DateTime SunDown) this[DateOnly index] => _neutralisationTimes[index];

        public DateTime GetSunUp(DateTime dateTime) => _neutralisationTimes[DateOnly.FromDateTime(dateTime)].SunUp;
        public DateTime GetSunUp(DateOnly date) => _neutralisationTimes[date].SunUp;
        public DateTime GetSunDown(DateTime dateTime) => _neutralisationTimes[DateOnly.FromDateTime(dateTime)].SunDown;
        public DateTime GetSunDown(DateOnly date) => _neutralisationTimes[date].SunDown;

        public TimeSpan GetNeutralizedTime(DateTime rawArrivalTime, DateTime startTime)
        {
            TimeSpan neutralizationPeriod = TimeSpan.Zero;
            foreach (int dayAfterStart in Enumerable.Range(1, rawArrivalTime.DayOfYear - startTime.DayOfYear))
            {
                neutralizationPeriod += GetSunUp(startTime.AddDays(dayAfterStart)) - GetSunDown(startTime.AddDays(dayAfterStart - 1)) - (_gracePeriod * 2);
            }

            if (GetSunUp(rawArrivalTime).Add(-_gracePeriod) > rawArrivalTime)
            {
                return (rawArrivalTime - startTime) * ((GetSunDown(rawArrivalTime.AddDays(-1)) - startTime) / (GetSunUp(rawArrivalTime) - startTime));
            }
            else if (GetSunDown(rawArrivalTime).Add(_gracePeriod) < rawArrivalTime)
            {
                return (rawArrivalTime - startTime) * ((GetSunDown(rawArrivalTime) - startTime) / (GetSunUp(rawArrivalTime.AddDays(1)) - startTime));
            }
            else
            {
                return rawArrivalTime - startTime - neutralizationPeriod;
            }
        }

        public bool IsInNeutralized(DateTime arrivalTime) => GetSunUp(arrivalTime) > arrivalTime || GetSunDown(arrivalTime) < arrivalTime;
    }
}
