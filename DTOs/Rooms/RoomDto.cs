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
        public EnemyDto? Enemy { get; init; }

        public int CoordX { get; init; }
        public int CoordY { get; init; }

        public bool HasLeftNeighbor { get; set; }
        public bool HasRightNeighbor { get; set; }
        public bool HasUpNeighbor { get; set; }
        public bool HasDownNeighbor { get; set; }
    }
}
