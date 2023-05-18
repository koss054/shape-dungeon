namespace ShapeDungeon.DTOs.Room
{
    public class RoomNavDto
    {
        public int CoordX { get; init; }
        public int CoordY { get; init; }
        public bool CanGoLeft { get; set; }
        public bool CanGoRight { get; set; }
        public bool CanGoUp { get; set; }
        public bool CanGoDown { get; set; }
        public bool HasLeftNeighbor { get; set; }
        public bool HasRightNeighbor { get; set; }
        public bool HasUpNeighbor { get; set; }
        public bool HasDownNeighbor { get; set; }
    }
}
