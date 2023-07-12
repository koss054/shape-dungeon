using ShapeDungeon.DTOs.Enemies;
using ShapeDungeon.DTOs.Players;
using ShapeDungeon.Entities;

namespace ShapeDungeon.DTOs
{
    public class CombatDto
    {
        public bool IsPlayerAttacking { get; init; }
        public PlayerDto Player { get; init; }
            = null!;
        public int CurrentPlayerHp { get; init; }
        public int TotalPlayerHp { get; init; }

        public EnemyDto Enemy { get; init; }
            = null!;
        public int CurrentEnemyHp { get; init; }
        public int TotalEnemyHp { get; init; }

        public Guid CombatRoomId { get; init; }

        public static implicit operator CombatDto(Combat combat)
            => new()
            {
                IsPlayerAttacking = combat.IsPlayerAttacking,
                Player = combat.Player,
                CurrentPlayerHp = combat.CurrentPlayerHp,
                TotalPlayerHp = combat.TotalPlayerHp,
                Enemy = combat.Enemy,
                CurrentEnemyHp = combat.CurrentEnemyHp,
                TotalEnemyHp = combat.TotalEnemyHp,
                CombatRoomId = combat.CombatRoomId,
            };
    }
}
