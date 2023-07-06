using ShapeDungeon.DTOs.Enemies;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Services.Enemies;
using ShapeDungeon.Repos;

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
            var enemies = await _enemyRepository.GetRangeAsync(minLevel, maxLevel);
            var enemyDtos = new List<EnemyRangeDto>();

            foreach (var enemy in enemies)
                enemyDtos.Add(enemy);

            return enemyDtos;
        }

        public async Task<Enemy> GetById(Guid enemyId)
        {
            var enemy = await _enemyRepository.GetById(enemyId);
            if (enemy == null) throw new ArgumentNullException(nameof(enemy));
            return enemy;

        }
    }
}
