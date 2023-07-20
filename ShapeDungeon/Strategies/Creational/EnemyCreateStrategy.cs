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

        public Enemy CreateObject()
        {
            var enemy = new Enemy()
            {
                IsActiveForCombat = false,
                Name = $"{_enemyDto.Name} Lvl.{_enemyDto.Level}",
                Strength = _enemyDto.Strength,
                Vigor = _enemyDto.Vigor,
                Agility = _enemyDto.Agility,
                Level = _enemyDto.Level,
                DroppedExp = _enemyDto.Level * 10,
                CurrentHp = _enemyDto.Vigor + _enemyDto.Strength,
                Shape = _enemyDto.Shape,
            };

            return enemy;
        }
    }
}
