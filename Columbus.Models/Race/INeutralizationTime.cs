namespace Columbus.Models.Race
{
    /// <summary>
    /// Time span during which the flight is neutralized (time is stopped).
    /// </summary>
    public interface INeutralizationTime
    {
        TimeSpan GetNeutralizedTime(DateTime rawArrivalTime, DateTime startTime);
        bool IsInNeutralized(DateTime arrivalTime);
    }
}
