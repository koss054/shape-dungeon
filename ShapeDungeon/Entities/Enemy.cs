using ShapeDungeon.Entities.Enums;
using ShapeDungeon.Interfaces.Entity;

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

        public int CurrentHp { get; set; }

        public Shape Shape { get; set; }
    }
}
