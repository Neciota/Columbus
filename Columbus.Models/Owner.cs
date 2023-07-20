namespace Columbus.Models
{
    /// <summary>
    /// Class <c>Owner</c> models a pigeon owner.
    /// </summary>
    public class Owner
    {
        /// <summary>
        /// Create an <c>Owner</c> from NPO-given ID, name, loft coordinate, and club ID.
        /// </summary>
        public Owner(int id, string name, Coordinate? coordinate, int club)
        {
            ID = id;
            Name = name;
            Coordinate = coordinate;
            Club = club;
        }

        /// <summary>
        /// Represent the 8-digt NPO-given ID, including club ID.
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Represent the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Represent the coordinate of the loft.
        /// </summary>
        public Coordinate? Coordinate { get; private set; }

        /// <summary>
        /// Represent the 4-digit club ID.
        /// </summary>
        public int Club { get; private set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
