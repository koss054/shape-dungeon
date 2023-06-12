using ShapeDungeon.DTOs.Enemies;
using ShapeDungeon.DTOs.Players;
using ShapeDungeon.DTOs.Rooms;

namespace ShapeDungeon.DTOs
{
    public class GameDto
    {
        public PlayerDto Player { get; set; }
            = null!;

        public RoomDto Room { get; set; }
            = null!;

        public EnemyDto? Enemy { get; set; }
    }
}
