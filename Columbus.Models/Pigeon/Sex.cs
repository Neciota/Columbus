using System.Runtime.CompilerServices;

namespace Columbus.Models.Pigeon
{
    /// <summary>
    /// Enum <c>Sex</c> models a pigeon's biological sex.
    /// </summary>
    public enum Sex
    {
        Unknown,
        Male,
        Female
    }

    public static class SexExtensions
    {
        public static string ToString(this Sex sex) => sex switch
        {
            Sex.Male => "M",
            Sex.Female => "V",
            _ => " ",
        };

        public static Sex Parse(ReadOnlySpan<char> s) => s switch
        {
            "M" => Sex.Male,
            "V" => Sex.Female,
            " " or "J" => Sex.Unknown,
            _ => throw new NotImplementedException()
        };
    }
}
