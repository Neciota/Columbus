﻿namespace Columbus.Models.Race
{
    using Columbus.Models.Owner;
    using Columbus.Models.Pigeon;

    public class PigeonRace(Pigeon pigeon, OwnerId ownerId, DateTime? arrivalTime, int mark)
    {
        public PigeonRace(Pigeon pigeon, OwnerId ownerId, DateTime? arrivalTime, int mark, int? points, int? position, int? next) : this(pigeon, ownerId, arrivalTime, mark)
        {
            Points = points;
            Position = position;
            Next = next;
        }

        public Pigeon Pigeon { get; set; } = pigeon;

        public OwnerId OwnerId { get; set; } = ownerId;

        public DateTime? ArrivalTime { get; set; } = arrivalTime;

        public int Mark { get; set; } = mark;

        public int? Points { get; set; }

        public int? Position { get; set; }

        public int? Next { get; set; }

        /// <summary>
        /// Gets the speed of a pigeon in the race.
        /// </summary>
        /// <param name="distance">Distance in meters.</param>
        /// <param name="startTime">Date/time the race started.</param>
        /// <param name="deviation">Owner's clock deviation from the atomic clock.</param>
        /// <returns>Speed of the pigeon adjusted for deviation.</returns>
        public Speed GetSpeed(double distance, DateTime startTime, TimeSpan deviation)
        {
            if (ArrivalTime is not null)
            {
                TimeSpan time = ArrivalTime.Value + deviation - startTime;
                return new(distance / time.TotalSeconds);
            }
            else
            {
                return Speed.Zero;
            }
        }
    }
}
