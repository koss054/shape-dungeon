using ShapeDungeon.Data;
using ShapeDungeon.DTOs.Enemies;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Enemies;
using ShapeDungeon.Strategies.Creational;

namespace ShapeDungeon.Services.Enemies
{
    public class EnemyCreateService : IEnemyCreateService
    {
        private readonly IEnemyRepository _enemyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EnemyCreateService(
            IEnemyRepository enemyRepository,
            IUnitOfWork unitOfWork)
        {
            _enemyRepository = enemyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task CreateAsync(EnemyDto eDto)
        {
            var enemyCreateContext = new CreateContext<Enemy, EnemyDto>(
                new EnemyCreateStrategy(eDto));

            var enemy = enemyCreateContext.ExecuteStrategy();

            await _unitOfWork.Commit(() =>
            {
                _enemyRepository.AddAsync(enemy);
            });
        }
    }
}
