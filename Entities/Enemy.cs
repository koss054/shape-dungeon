using ShapeDungeon.Interfaces.Entity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShapeDungeon.Entities
{
    public class Enemy : IEnemy
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
            = null!;

        public int Strength { get; set; }

        public int Vigor { get; set; }

        public int Agility { get; set; }

        public int Level { get; set; }

        public int DroppedExp { get; set; }

        [ForeignKey(nameof(Room))]
        public Guid RoomId { get; set; }

        public Room? Room { get; set; }
    }
}
