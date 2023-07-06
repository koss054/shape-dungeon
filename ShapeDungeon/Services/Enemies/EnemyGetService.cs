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

        // TODO: Make exceptions return better info.
        public async Task<Enemy> GetById(Guid enemyId)
        {
            var enemy = await _enemyRepository.GetById(enemyId);
            return enemy ?? throw new ArgumentNullException("Enemy id.", 
                "No entity matches provided id.");
        }

        public async Task<Enemy> GetIsActiveForCombat()
        {
            var enemy = await _enemyRepository.GetActiveForCombat();
            return enemy ?? throw new ArgumentNullException("Enemy for combat.", 
                "No active combat enemy when expected.");
        }
    }
}
