using ShapeDungeon.DTOs.Enemies;
using ShapeDungeon.Entities;

namespace ShapeDungeon.Interfaces.Services.Enemies
{
    public interface IEnemyGetService
    {
        Task<IEnumerable<EnemyRangeDto>> GetRangeAsync(int minLevel, int maxLevel);
        Task<Enemy> GetById(Guid enemyId);
    }
}
