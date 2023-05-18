namespace ShapeDungeon.DTOs.Room
{
    public class RoomCreateDto
    {
        public RoomDetailsDto Details { get; set; }
            = null!;

        public RoomNavDto? Nav { get; set; }

    }
}
