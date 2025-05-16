using Columbus.Models;
using Columbus.Models.Owner;
using System.Globalization;

namespace Columbus.UDP.Lines
{
    internal class ClockDeviationLine : IUdpLine
    {
        private const int ClubStart = 3;
        private const int ClubLength = 4;
        private const int OwnerStart = 7;
        private const int OwnerLength = 8;
        private const int SubmissionAtomicClockStart = 17;
        private const int SubmissionAtomicClockLength = 10;
        private const int SubmissionOwnerClockStart = 27;
        private const int SubmissionOwnerClockLength = 10;
        private const int StopAtomicClockTimeStart = 37;
        private const int StopAtomicClockTimeLength = 10;
        private const int StopOwnerClockTimeStart = 47;
        private const int StopOwnerClockTimeLength = 10;
        private const int HashStart = 79;
        private const int HashLength = 8;

        public LineType Type { get; } = LineType.ClockDeviation;
        public ClubId ClubId { get; private set; }
        public OwnerId OwnerId { get; private set; }
        public DateTime SubmissionAtomicClockTime { get; private set; }
        public DateTime SubmissionOwnerClockTime { get; private set; }
        public DateTime StopAtomicClockTime { get; private set; }
        public DateTime StopOwnerClockTime { get; private set; }
        public int Hash { get; private set; }

        public void Deserialize(string line)
        {
            ClubId = ClubId.Parse(line.AsSpan(ClubStart, ClubLength), CultureInfo.InvariantCulture);
            OwnerId = OwnerId.Parse(line.AsSpan(OwnerStart, OwnerLength), CultureInfo.InvariantCulture);
            SubmissionAtomicClockTime = DateTime.ParseExact(line.AsSpan(SubmissionAtomicClockStart, SubmissionAtomicClockLength), "ddMMHHmmss", CultureInfo.InvariantCulture);
            SubmissionOwnerClockTime = DateTime.ParseExact(line.AsSpan(SubmissionOwnerClockStart, SubmissionOwnerClockLength), "ddMMHHmmss", CultureInfo.InvariantCulture);
            StopAtomicClockTime = DateTime.ParseExact(line.AsSpan(StopAtomicClockTimeStart, StopAtomicClockTimeLength), "ddMMHHmmss", CultureInfo.InvariantCulture);
            StopOwnerClockTime = DateTime.ParseExact(line.AsSpan(StopOwnerClockTimeStart, StopOwnerClockTimeLength), "ddMMHHmmss", CultureInfo.InvariantCulture);
            Hash = int.Parse(line.AsSpan(HashStart, HashLength), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
