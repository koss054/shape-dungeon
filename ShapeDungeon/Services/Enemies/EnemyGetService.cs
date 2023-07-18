using ShapeDungeon.DTOs.Enemies;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Enemies;
using ShapeDungeon.Repos;
using ShapeDungeon.Specifications.Enemies;

namespace ShapeDungeon.Services.Enemies
{
    public class EnemyGetService : IEnemyGetService
    {
        private readonly IEnemyRepository _enemyRepository;

        public EnemyGetService(IEnemyRepository enemyRepository)
        {
            _enemyRepository = enemyRepository;
        }

        public async Task<IEnumerable<EnemyRangeDto>> GetRangeAsync(int minLevel, int maxLevel)
        {
            var enemies = await _enemyRepository.GetMultipleByAsync(
                new EnemyLevelRangeSpecification(minLevel, maxLevel));

            var enemyDtos = new List<EnemyRangeDto>();

            foreach (var enemy in enemies)
                enemyDtos.Add(enemy);

            return enemyDtos;
        }

        // TODO: Make exceptions return better info.
        public async Task<Enemy> GetById(Guid enemyId)
        {
            var enemy = await _enemyRepository.GetFirstAsync(
                new EnemyIdSpecification(enemyId));

            return enemy;
        }

        public async Task<Enemy> GetIsActiveForCombat()
        {
            var enemy = await _enemyRepository.GetFirstAsync(
                new EnemyActiveForCombatSpecification());

            return enemy;
        }

        public async Task<int> GetActiveForCombatExp()
        {
            var enemy = await _enemyRepository.GetFirstAsync(
                new EnemyActiveForCombatSpecification());

            return enemy.DroppedExp;
        }
    }
}
