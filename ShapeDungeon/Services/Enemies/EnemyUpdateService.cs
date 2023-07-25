using ShapeDungeon.Data;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Enemies;
using ShapeDungeon.Specifications.Enemies;

namespace ShapeDungeon.Services.Enemies
{
    public class EnemyUpdateService : IEnemyUpdateService
    {
        private readonly IEnemyRepository _enemyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EnemyUpdateService(
            IEnemyRepository enemyRepository, 
            IUnitOfWork unitOfWork)
        {
            _enemyRepository = enemyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task RemoveActiveForCombat()
        {
            var enemy = await _enemyRepository.GetFirstAsync(
                new EnemyActiveForCombatSpecification());

            enemy.IsActiveForCombat = false;

            await _unitOfWork.Commit(() =>
            {
                _enemyRepository.Update(enemy);
            });
        }
    }
}
