using ShapeDungeon.Interfaces.Entity;

namespace ShapeDungeon.Entities
{
    public class Room : IRoom
    {
        public Guid Id { get; init; }

        public bool IsActive { get; set; }

        public bool IsActiveForEdit { get; set; }

        public bool CanGoLeft { get; set; }

        public bool CanGoRight { get; set; }

        public bool CanGoUp { get; set; }

        public bool CanGoDown { get; set; }

        public bool IsStartRoom { get; init; }

        public bool IsEnemyRoom { get; init; }

        public bool IsSafeRoom { get; init; }

        public bool IsEndRoom { get; init; }

        public Enemy? Enemy { get; init; }

        public Guid? LeftRoomId { get; init; }
        public Guid? RightRoomId { get; init; }
        public Guid? TopRoomId { get; init; }
        public Guid? DownRoomId { get; init; }
    }
}
