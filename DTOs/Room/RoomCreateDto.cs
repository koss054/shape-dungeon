namespace ShapeDungeon.DTOs.Room
{
    public class RoomCreateDto
    {
        public Guid Id { get; set; }
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
    }
}
