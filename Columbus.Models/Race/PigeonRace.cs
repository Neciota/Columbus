namespace Columbus.Models.Race
{
    using Columbus.Models.Owner;
    using Columbus.Models.Pigeon;

    public class PigeonRace(Pigeon pigeon, OwnerId ownerId, DateTime? arrivalTime, int mark, int arrivalOrder)
    {
        public PigeonRace(Pigeon pigeon, OwnerId ownerId, DateTime? arrivalTime, int mark, int arrivalOrder, int? points, int? position, int? next) : this(pigeon, ownerId, arrivalTime, mark, arrivalOrder)
        {
            Points = points;
            Position = position;
            Next = next;
        }

        public Pigeon Pigeon { get; set; } = pigeon;

        public OwnerId OwnerId { get; set; } = ownerId;

        public DateTime? ArrivalTime { get; set; } = arrivalTime;

        public int Mark { get; set; } = mark;

        public int ArrivalOrder { get; set; } = arrivalOrder;

        public double? Points { get; set; }

        public int? Position { get; set; }

        public int? Next { get; set; }

        public DateTime? GetCorrectedArrivalTime(DateTime? submissionTime, DateTime? stopTime, TimeSpan deviation)
        {
            if (!ArrivalTime.HasValue)
                return null;

            TimeSpan submissionToArrival = ArrivalTime.Value - submissionTime ?? TimeSpan.Zero;
            TimeSpan submissionToStop = stopTime - submissionTime ?? TimeSpan.Zero;
            double deviationFactor = deviation.Divide(submissionToStop);
            TimeSpan arrivalDeviation = submissionToArrival * deviationFactor;

            return ArrivalTime + arrivalDeviation;
        }

        /// <summary>
        /// Gets the speed of a pigeon in the race.
        /// </summary>
        /// <param name="distance">Distance in meters.</param>
        /// <param name="startTime">Date/time the race started.</param>
        /// <param name="deviation">Owner's clock deviation from the atomic clock.</param>
        /// <returns>Speed of the pigeon adjusted for deviation.</returns>
        public Speed GetSpeed(double distance, DateTime startTime, DateTime? submissionTime, DateTime? stopTime, TimeSpan deviation, INeutralizationTime neutralisationTime)
        {
            DateTime? correctedArrivalTime = GetCorrectedArrivalTime(submissionTime, stopTime, deviation);
            if (!correctedArrivalTime.HasValue)
                return Speed.Zero;

            TimeSpan time = neutralisationTime.GetNeutralizedTime(correctedArrivalTime.Value, startTime);
            return new(distance / time.TotalSeconds);
        }

        public bool IsInNeutralized(INeutralizationTime neutralizationTime) => ArrivalTime.HasValue && neutralizationTime.IsInNeutralized(ArrivalTime.Value);
    }
}
