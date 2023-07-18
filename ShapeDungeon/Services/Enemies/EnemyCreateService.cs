using ShapeDungeon.Data;
using ShapeDungeon.DTOs.Enemies;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Services.Enemies;
using ShapeDungeon.Repos;

namespace ShapeDungeon.Services.Enemies
{
    public class EnemyCreateService : IEnemyCreateService
    {
        private readonly IEnemyRepositoryOld _enemyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EnemyCreateService(
            IEnemyRepositoryOld enemyRepository,
            IUnitOfWork unitOfWork)
        {
            _enemyRepository = enemyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(EnemyDto eDto)
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

            await _unitOfWork.Commit(() =>
            {
                _enemyRepository.AddAsync(enemy);
            });
        }
    }
}
