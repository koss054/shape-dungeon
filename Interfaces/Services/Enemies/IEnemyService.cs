using ShapeDungeon.DTOs.Enemies;

namespace ShapeDungeon.Interfaces.Services.Enemies
{
    public interface IEnemyService
    {
        Task CreateAsync(EnemyDto eDto);
        Task<IEnumerable<EnemyRangeDto>> GetRangeAsync(int minLevel, int maxLevel);
    }
}
