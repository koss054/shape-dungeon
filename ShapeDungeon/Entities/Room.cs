using ShapeDungeon.Interfaces.Entity;

namespace ShapeDungeon.Entities
{
    public class Room : IRoom
    {
        public Guid Id { get; init; }

        public bool IsActiveForMove { get; set; }

        public bool IsActiveForScout { get; set; }

        public bool IsActiveForEdit { get; set; }

        public bool CanGoLeft { get; set; }

        public bool CanGoRight { get; set; }

        public bool CanGoUp { get; set; }

        public bool CanGoDown { get; set; }

        // TODO: Change these four variables to an enum.
        public bool IsStartRoom { get; init; }

        public bool IsEnemyRoom { get; init; }

        public bool IsSafeRoom { get; init; }

        public bool IsEndRoom { get; init; }

        public int CoordX { get; set; }
        public int CoordY { get; set; }
    }
}
