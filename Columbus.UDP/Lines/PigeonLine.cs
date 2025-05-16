using Columbus.Models;
using Columbus.Models.Owner;
using Columbus.Models.Pigeon;
using System.Globalization;

namespace Columbus.UDP.Lines
{
    internal class PigeonLine : IUdpLine
    {
        private const int ClubIdStart = 3;
        private const int ClubIdLength = 4;
        private const int OwnerIdStart = 7;
        private const int OwnerIdLength = 8;
        private const int SexStart = 15;
        private const int SexLength = 1;
        private const int CountryStart = 16;
        private const int CountryLength = 4;
        private const int YearStart = 20;
        private const int YearLength = 2;
        private const int RingNumberStart = 25;
        private const int RingNumberLength = 7;
        private const int MarkStart = 32;
        private const int MarkLength = 3;
        private const int ArrivedStart = 48;
        private const int ArrivedLength = 1;
        private const int ArrivalOrderStart = 49;
        private const int ArrivalOrderLength = 3;
        private const int ArrivalStart = 52;
        private const int ArrivalLength = 10;
        private const int ChipStart = 79;
        private const int ChipLength = 8;
        private const int SubmittedAtStart = 87;
        private const int SubmittedAtLength = 10;
        private const int VaccinationDateStart = 148;
        private const int VaccinationDateLength = 6;
        private const int HashStart = 154;
        private const int HashLength = 8;

        public LineType Type { get; } = LineType.Pigeon;
        public ClubId ClubId { get; private set; }
        public OwnerId OwnerId { get; private set; }
        public Sex Sex { get; private set; }
        public CountryCode Country { get; private set; }
        public int Year { get; private set; }
        public RingNumber RingNumber { get; private set; }
        public int Mark { get; private set; }
        public bool Arrived { get; private set; }
        public int ArrivalOrder { get; private set; }
        public DateTime? Arrival { get; private set; }
        public int Chip { get; private set; }
        public DateTime SubmittedAt { get; private set; }
        public DateTime VaccinationDate { get; private set; }
        public int Hash { get; private set; }

        public void Deserialize(string line)
        {
            ClubId = ClubId.Parse(line.AsSpan(ClubIdStart, ClubIdLength), CultureInfo.InvariantCulture);
            OwnerId = OwnerId.Parse(line.AsSpan(OwnerIdStart, OwnerIdLength), CultureInfo.InvariantCulture);
            Sex = SexExtensions.Parse(line.AsSpan(SexStart, SexLength));
            Country = CountryCode.Parse(line.AsSpan(CountryStart, CountryLength), CultureInfo.InvariantCulture);
            Year = int.Parse(line.AsSpan(YearStart, YearLength), CultureInfo.InvariantCulture);
            RingNumber = RingNumber.Parse(line.AsSpan(RingNumberStart, RingNumberLength), CultureInfo.InvariantCulture);
            Mark = int.Parse(line.AsSpan(MarkStart, MarkLength), CultureInfo.InvariantCulture);
            Arrived = Convert.ToBoolean(int.Parse(line.AsSpan(ArrivedStart, ArrivedLength)));
            ArrivalOrder = int.Parse(line.AsSpan(ArrivalOrderStart, ArrivalOrderLength), CultureInfo.InvariantCulture);
            if (DateTime.TryParseExact(line.AsSpan(ArrivalStart, ArrivalLength), "ddMMHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime arrival))
                Arrival = arrival;
            Chip = int.Parse(line.AsSpan(ChipStart, ChipLength), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            SubmittedAt = DateTime.ParseExact(line.AsSpan(SubmittedAtStart, SubmittedAtLength), "ddMMHHmmss", CultureInfo.InvariantCulture);
            VaccinationDate = DateTime.ParseExact(line.AsSpan(VaccinationDateStart, VaccinationDateLength), "ddMMyy", CultureInfo.InvariantCulture);
            Hash = int.Parse(line.AsSpan(HashStart, HashLength), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
