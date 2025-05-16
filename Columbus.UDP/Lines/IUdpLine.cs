namespace Columbus.UDP.Lines
{
    internal interface IUdpLine
    {
        public const int TypeStart = 0;
        public const int TypeLength = 1;

        LineType Type { get; }
        int Hash { get; }

        string Serialize();
        void Deserialize(string line);
    }
}
