using ShapeDungeon.DTOs.Enemies;
using ShapeDungeon.DTOs.Players;
using ShapeDungeon.Entities;

namespace ShapeDungeon.DTOs
{
    public class CombatDto
    {
        public PlayerDto Player { get; init; }
            = null!;

        public EnemyDto Enemy { get; init; }
            = null!;

        public Guid CombatRoomId { get; init; }

        public static implicit operator CombatDto(Combat combat)
            => new()
            {
                Player = combat.Player,
                Enemy = combat.Enemy,
                CombatRoomId = combat.CombatRoomId,
            };
    }
}
