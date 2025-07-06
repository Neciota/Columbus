using Columbus.Models;
using Columbus.Models.Owner;
using System.Globalization;

namespace Columbus.UDP.Lines.Race
{
    internal class OwnerLine : IUdpLine
    {
        private const int ClubStart = 3;
        private const int ClubLength = 4;
        private const int IdStart = 7;
        private const int IdLength = 8;
        private const int LoftLocationStart = 29;
        private const int LoftLocationLength = 20;
        private const int NameStart = 50;
        private const int NameLength = 20;
        private const int StreetAndHouseStart = 70;
        private const int StreetAndHouseLength = 30;
        private const int PostCodeStart = 100;
        private const int PostCodeLength = 7;
        private const int CountryStart = 107;
        private const int CountryLength = 2;
        private const int TownStart = 111;
        private const int TownLength = 22;
        private const int TelephoneNumberStart = 147;
        private const int TelephoneNumberLength = 15;
        private const int EmailStart = 162;
        private const int EmailLength = 48;
        private const int HashLength = 8;

        public LineType Type { get; } = LineType.Owner;
        public ClubId Club { get; private set; }
        public OwnerId Id { get; private set; }
        public Coordinate LoftLocation { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public Address Address { get; private set; }
        public string TelephoneNumber { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public int Hash { get; private set; }

        public void Deserialize(string line)
        {
            Club = ClubId.Parse(line.AsSpan(ClubStart, ClubLength), CultureInfo.InvariantCulture);
            Id = OwnerId.Parse(line.AsSpan(IdStart, IdLength), CultureInfo.InvariantCulture);
            if (Coordinate.TryParseFromDms(line.AsSpan(LoftLocationStart, LoftLocationLength), CultureInfo.InvariantCulture, out Coordinate loftCoordinate))
                LoftLocation = loftCoordinate;
            Name = line.Substring(NameStart, NameLength).Trim();
            Address = new(
                line.Substring(StreetAndHouseStart, StreetAndHouseLength).Trim(),
                line.Substring(PostCodeStart, PostCodeLength).Trim(),
                line.Substring(CountryStart, CountryLength).Trim(),
                line.Substring(TownStart, TownLength).Trim()
            );
            TelephoneNumber = line.Substring(TelephoneNumberStart, TelephoneNumberLength).Trim();
            Email = line.Substring(EmailStart, EmailLength).Trim();
            Hash = int.Parse(line.AsSpan(line.Length - HashLength, HashLength), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
