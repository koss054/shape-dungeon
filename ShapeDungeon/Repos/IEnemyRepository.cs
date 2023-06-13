using ShapeDungeon.Entities;

namespace ShapeDungeon.Repos
{
    public interface IEnemyRepository
    {
        Task AddAsync(Enemy enemy);
    }
}
