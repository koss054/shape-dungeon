using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;

namespace ShapeDungeon.DTOs.Enemies
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
        public int CurrentHp { get; set; }
        public Shape Shape { get; set; }

        public static implicit operator EnemyDto(Enemy enemy)
            => new()
            {
                Name = enemy.Name,
                Strength = enemy.Strength,
                Vigor = enemy.Vigor,
                Agility = enemy.Agility,
                Level = enemy.Level,
                DroppedExp = enemy.DroppedExp,
                CurrentHp = enemy.CurrentHp,
                Shape = enemy.Shape,
            };
    }
}
