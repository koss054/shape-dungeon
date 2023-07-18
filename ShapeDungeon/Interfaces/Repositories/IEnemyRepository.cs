using ShapeDungeon.Entities;

namespace ShapeDungeon.Interfaces.Repositories
{
    public interface IEnemyRepository : IRepositoryGet<Enemy>, IRepositoryValidate<Enemy>
    {
    }
}
