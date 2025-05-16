namespace Columbus.Models.Owner
{
    using Columbus.Models.Pigeon;

    /// <summary>
    /// Class <c>Owner</c> models a pigeon owner.
    /// </summary>
    public class Owner
    {
        /// <summary>
        /// Create an <c>Owner</c> from NPO-given ID, name, loft coordinate, and club ID.
        /// </summary>
        public Owner(OwnerId id, string name, Coordinate loftCoordinate, ClubId club)
        {
            Id = id;
            Name = name;
            LoftCoordinate = loftCoordinate;
            Club = club;
            Pigeons = new List<Pigeon>();
        }

        /// <summary>
        /// Create an <c>Owner</c> from NPO-given ID, name, loft coordinate, and club ID, and owned pigeons.
        /// </summary>
        public Owner(OwnerId id, string name, Coordinate loftCoordinate, ClubId club, IList<Pigeon> pigeons)
        {
            Id = id;
            Name = name;
            LoftCoordinate = loftCoordinate;
            Club = club;
            Pigeons = pigeons;
        }

        /// <summary>
        /// Represents the 8-digt NPO-given ID, including club ID.
        /// </summary>
        public OwnerId Id { get; private set; }

        /// <summary>
        /// Represent the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Represents the coordinate of the loft.
        /// </summary>
        public Coordinate LoftCoordinate { get; private set; }

        /// <summary>
        /// Represents the 4-digit club ID.
        /// </summary>
        public ClubId Club { get; private set; }

        /// <summary>
        /// Represents the pigeons owned by this owner.
        /// </summary>
        public IList<Pigeon> Pigeons { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
