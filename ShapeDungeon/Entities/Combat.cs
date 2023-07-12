using System.ComponentModel.DataAnnotations.Schema;

namespace ShapeDungeon.Entities
{
    public class Combat
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsPlayerAttacking { get; set; }

        [ForeignKey(nameof(Player))]
        public Guid PlayerId { get; set; }
        public Player Player { get; set; }
            = null!;
        public int CurrentPlayerHp { get; set; }
        public int TotalPlayerHp { get; set; }

        [ForeignKey(nameof(Enemy))]
        public Guid EnemyId { get; set; }
        public Enemy Enemy { get; set; }
            = null!;
        public int CurrentEnemyHp { get; set; }
        public int TotalEnemyHp { get; set; }

        public Guid CombatRoomId { get; set; }
    }
}
