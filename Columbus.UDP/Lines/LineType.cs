namespace Columbus.UDP.Lines
{
    internal enum LineType
    {
        Header = 0,
        Owner = 1,
        LevelEntry = 2,
        ClockDeviation = 3,
        Pigeon = 4,
        Footer = 9,
    }
}
