using ShapeDungeon.DTOs.Enemies;
using ShapeDungeon.Entities;

namespace ShapeDungeon.DTOs.Rooms
{
    public class RoomDto
    {
        public bool IsActiveForMove { get; set; }
        public bool IsActiveForScout { get; set; }
        public bool CanGoLeft { get; set; }
        public bool CanGoRight { get; set; }
        public bool CanGoUp { get; set; }
        public bool CanGoDown { get; set; }
        public bool IsStartRoom { get; init; }
        public bool IsEnemyRoom { get; init; }
        public bool IsSafeRoom { get; init; }
        public bool IsEndRoom { get; init; }
        public EnemyDto? Enemy { get; set; }

        public int CoordX { get; init; }
        public int CoordY { get; init; }

        public bool HasLeftNeighbor { get; set; }
        public bool HasRightNeighbor { get; set; }
        public bool HasUpNeighbor { get; set; }
        public bool HasDownNeighbor { get; set; }
        public bool IsLeftDeadEnd { get; set; }
        public bool IsRightDeadEnd { get; set; }
        public bool IsUpDeadEnd { get; set; }
        public bool IsDownDeadEnd { get; set; }

        public bool IsEnemyDefeated { get; set; }

        public static implicit operator RoomDto(Room room)
            => new()
            {
                IsActiveForMove = room.IsActiveForMove,
                IsActiveForScout = room.IsActiveForScout,
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
