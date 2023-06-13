using ShapeDungeon.Entities.Enums;

namespace ShapeDungeon.DTOs
{
    public class EnemyDto
    {
        public string Name { get; set; }
            = null!;

        public int Strength { get; set; }
        public int Vigor { get; set; }
        public int Agility { get; set; }
        public int Level { get; set; }
        public int DroppedExp { get; set; }
        public Shape Shape { get; set; }
    }
}
