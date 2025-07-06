using Columbus.Models;
using Columbus.Models.Owner;
using Columbus.Models.Pigeon;
using System.Globalization;

namespace Columbus.UDP.Lines.Owner
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
        private const int ChipStart = 79;
        private const int ChipLength = 8;
        private const int VaccinationDateStart = 148;
        private const int VaccinationDateLength = 6;
        private const int ColorStart = 157;
        private const int ColorLength = 10;
        private const int HashStart = 176;
        private const int HashLength = 8;

        public LineType Type { get; } = LineType.Pigeon;
        public ClubId ClubId { get; private set; }
        public OwnerId OwnerId { get; private set; }
        public Sex Sex { get; private set; }
        public CountryCode Country { get; private set; }
        public int Year { get; private set; }
        public RingNumber RingNumber { get; private set; }
        public int Chip { get; private set; }
        public DateTime VaccinationDate { get; private set; }
        public string Color { get; set; } = string.Empty;
        public int Hash { get; private set; }

        public void Deserialize(string line)
        {
            ClubId = ClubId.Parse(line.AsSpan(ClubIdStart, ClubIdLength), CultureInfo.InvariantCulture);
            OwnerId = OwnerId.Parse(line.AsSpan(OwnerIdStart, OwnerIdLength), CultureInfo.InvariantCulture);
            Sex = SexExtensions.Parse(line.AsSpan(SexStart, SexLength));
            Country = CountryCode.Parse(line.AsSpan(CountryStart, CountryLength), CultureInfo.InvariantCulture);
            Year = int.Parse(line.AsSpan(YearStart, YearLength), CultureInfo.InvariantCulture);
            RingNumber = RingNumber.Parse(line.AsSpan(RingNumberStart, RingNumberLength), CultureInfo.InvariantCulture);
            Chip = int.Parse(line.AsSpan(ChipStart, ChipLength), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            if (DateTime.TryParseExact(line.AsSpan(VaccinationDateStart, VaccinationDateLength), "ddMMyy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime vaccinationDate))
                VaccinationDate = vaccinationDate;
            Color = line.Substring(ColorStart, ColorLength);
            Hash = int.Parse(line.AsSpan(HashStart, HashLength), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
