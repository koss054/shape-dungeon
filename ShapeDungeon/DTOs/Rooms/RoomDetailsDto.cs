using ShapeDungeon.Entities;

namespace ShapeDungeon.DTOs.Rooms
{
    public class RoomDetailsDto
    {
        public bool IsActiveForEdit { get; set; }
        public bool CanGoLeft { get; set; }
        public bool CanGoRight { get; set; }
        public bool CanGoUp { get; set; }
        public bool CanGoDown { get; set; }
        public bool IsStartRoom { get; init; }
        public bool IsEnemyRoom { get; init; }
        public bool IsSafeRoom { get; init; }
        public bool IsEndRoom { get; init; }
        public EnemyDto? Enemy { get; init; }

        public int CoordX { get; set; }
        public int CoordY { get; set; }

        public static implicit operator RoomDetailsDto(Room room)
            => new()
            {
                IsActiveForEdit = room.IsActiveForEdit,
                CanGoLeft = room.CanGoLeft,
                CanGoRight = room.CanGoRight,
                CanGoUp = room.CanGoUp,
                CanGoDown = room.CanGoDown,
                IsStartRoom = room.IsStartRoom,
                IsEnemyRoom = room.IsEnemyRoom,
                IsSafeRoom = room.IsSafeRoom,
                IsEndRoom = room.IsEndRoom,
                Enemy = null,
                CoordX = room.CoordX,
                CoordY = room.CoordY,
            };
    }
}
