namespace ShapeDungeon.DTOs
{
    public class RoomNavDto
    {
        public Guid Id { get; init; }
        public Guid? LeftRoomId { get; init; }
        public Guid? RightRoomId { get; init; }
        public Guid? TopRoomId { get; init; }
        public Guid? DownRoomId { get; init; }
    }
}
