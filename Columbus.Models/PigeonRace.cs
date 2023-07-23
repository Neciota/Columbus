namespace Columbus.Models
{
    public class PigeonRace
    {
        public PigeonRace(Pigeon pigeon, DateTime arrivalTime, int mark)
        {
            Pigeon = pigeon;
            ArrivalTime = arrivalTime;
            Mark = mark;
        }

        public Pigeon Pigeon { get; set; }

        public DateTime ArrivalTime { get; set; }

        public int Mark { get; set; }

        public double? Speed { get; private set; }

        public int? Points { get; set; }

        public int? Position { get; set; }

        public int? Next { get; set; }

        public double CalculateSpeed(double distance, DateTime startTime)
        {
            if (ArrivalTime.Year > 1)
            {
                TimeSpan time = ArrivalTime - startTime;
                Speed = distance / time.TotalMinutes; //meters per minute is a sane format
            }
            else
                Speed = 0;

            return Speed.Value;
        }
    }
}
