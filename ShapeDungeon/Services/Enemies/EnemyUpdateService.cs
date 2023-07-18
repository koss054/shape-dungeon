using ShapeDungeon.Data;
using ShapeDungeon.Interfaces.Services.Enemies;
using ShapeDungeon.Repos;

namespace ShapeDungeon.Services.Enemies
{
    public class EnemyUpdateService : IEnemyUpdateService
    {
        private readonly IEnemyRepositoryOld _enemyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EnemyUpdateService(
            IEnemyRepositoryOld enemyRepository, 
            IUnitOfWork unitOfWork)
        {
            _enemyRepository = enemyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task RemoveActiveForCombat()
        {
            var enemy = await _enemyRepository.GetActiveForCombat();
            if (enemy is null) throw new ArgumentNullException(nameof(enemy));

            await _unitOfWork.Commit(() =>
            {
                enemy.IsActiveForCombat = false;
            });
        }
    }
}
