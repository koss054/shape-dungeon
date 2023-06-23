using ShapeDungeon.Entities;

namespace ShapeDungeon.DTOs.Rooms
{
    public class RoomNavDto
    {
        public int CoordX { get; set; }
        public int CoordY { get; set; }
        public bool CanGoLeft { get; set; }
        public bool CanGoRight { get; set; }
        public bool CanGoUp { get; set; }
        public bool CanGoDown { get; set; }
        public bool HasLeftNeighbor { get; set; }
        public bool HasRightNeighbor { get; set; }
        public bool HasUpNeighbor { get; set; }
        public bool HasDownNeighbor { get; set; }
        public bool IsLeftDeadEnd { get; set; }
        public bool IsRightDeadEnd { get; set; }
        public bool IsUpDeadEnd { get; set; }
        public bool IsDownDeadEnd { get; set; }

        public static implicit operator RoomNavDto(Room room)
            => new()
            {
                CoordX = room.CoordX,
                CoordY = room.CoordY,
                CanGoLeft = room.CanGoLeft,
                CanGoRight = room.CanGoRight,
                CanGoUp = room.CanGoUp,
                CanGoDown = room.CanGoDown,
            };
    }
}
