using ShapeDungeon.DTOs.Enemies;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Strategies;

namespace ShapeDungeon.Strategies.Creational
{
    public class EnemyCreateStrategy : ICreateStrategy<Enemy, EnemyDto>
    {
        private readonly EnemyDto _enemyDto;

        public EnemyCreateStrategy(EnemyDto enemyDto)
        {
            _enemyDto = enemyDto;
        }

        public Enemy CreateObject(EnemyDto eDto)
        {
            var enemy = new Enemy()
            {
                IsActiveForCombat = false,
                Name = $"{eDto.Name} Lvl.{eDto.Level}",
                Strength = eDto.Strength,
                Vigor = eDto.Vigor,
                Agility = eDto.Agility,
                Level = eDto.Level,
                DroppedExp = eDto.Level * 10,
                CurrentHp = eDto.Vigor + eDto.Strength,
                Shape = eDto.Shape,
            };

            return enemy;
        }
    }
}
