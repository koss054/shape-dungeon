using ShapeDungeon.DTOs.Enemies;

namespace ShapeDungeon.DTOs.Rooms
{
    public class RoomCreateDto
    {
        public RoomDetailsDto Details { get; set; }
            = null!;

        public RoomNavDto? Nav { get; set; }

        public IEnumerable<EnemyRangeDto> EnemyRange { get; set; }
            = new List<EnemyRangeDto>();

        public int MinLevel { get; set; }
        public int MaxLevel { get; set; }
        public Guid EnemyId { get; set; }
    }
}
