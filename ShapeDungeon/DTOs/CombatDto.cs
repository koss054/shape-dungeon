using ShapeDungeon.DTOs.Enemies;
using ShapeDungeon.DTOs.Players;

namespace ShapeDungeon.DTOs
{
    public class CombatDto
    {
        public PlayerDto Player { get; init; }
            = null!;

        public EnemyDto Enemy { get; init; }
            = null!;

        public Guid CombatRoomId { get; init; }
    }
}
