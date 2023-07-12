using ShapeDungeon.Entities;

namespace ShapeDungeon.Responses.Enemies
{
    public class EnemyStatResponse
    {
        public int Strength { get; init; }
        public int Vigor { get; init; }
        public int Agility { get; init; }

        public static implicit operator EnemyStatResponse(Enemy enemy)
            => new()
            {
                Strength = enemy.Strength,
                Vigor = enemy.Vigor,
                Agility = enemy.Agility,
            };
    }
}
