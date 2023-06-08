using System.ComponentModel.DataAnnotations.Schema;

namespace ShapeDungeon.Entities
{
    public class EnemyRoom
    {
        [ForeignKey(nameof(Enemy))]
        public Guid EnemyId { get; set; }

        public Enemy Enemy { get; set; }
            = null!;

        [ForeignKey(nameof(Room))]
        public Guid RoomId { get; set; }

        public Room Room { get; set; }
            = null!;

        public bool IsEnemyDefeated { get; set; }
    }
}
