using ShapeDungeon.Entities;

namespace ShapeDungeon.Repos
{
    public interface IEnemyRepository
    {
        Task AddAsync(Enemy enemy);
        Task<IEnumerable<Enemy>> GetRangeAsync(int minLevel, int maxLevel);
        Task<Enemy?> GetById(Guid enemyId);
        Task<Enemy?> GetActiveForCombat();
        Task SetActiveForCombat(Guid enemyId);
        Task ClearActiveForCombat();
    }
}
