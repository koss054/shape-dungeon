namespace ShapeDungeon.DTOs.Rooms
{
    public class RoomCreateDto
    {
        public RoomDetailsDto Details { get; set; }
            = null!;

        public RoomNavDto? Nav { get; set; }

    }
}
