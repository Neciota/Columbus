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
            Pigeons = new List<Pigeon>();
        }

        /// <summary>
        /// Represents the 8-digt NPO-given ID, including club ID.
        /// </summary>
        public int ID { get; private set; }

        /// <summary>
        /// Represent the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Represents the coordinate of the loft.
        /// </summary>
        public Coordinate? Coordinate { get; private set; }

        /// <summary>
        /// Represents the 4-digit club ID.
        /// </summary>
        public int Club { get; private set; }

        /// <summary>
        /// Represents the pigeons owned by this owner.
        /// </summary>
        public IEnumerable<Pigeon> Pigeons { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
