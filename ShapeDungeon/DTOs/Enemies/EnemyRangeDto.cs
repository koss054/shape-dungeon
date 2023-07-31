using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;

namespace ShapeDungeon.DTOs.Enemies
{
    public class EnemyRangeDto
    {
        public Guid Id { get; set; }

        public string Name { get; init; }
            = null!;

        public int Level { get; init; }

        public Shape Shape { get; init; }

        public static implicit operator EnemyRangeDto(Enemy enemy)
            => new()
            {
                Id = enemy.Id,
                Name = enemy.Name,
                Level = enemy.Level,
                Shape = enemy.Shape,
            };
    }
}