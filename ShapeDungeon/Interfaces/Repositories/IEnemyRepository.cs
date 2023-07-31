using ShapeDungeon.Entities;

namespace ShapeDungeon.Interfaces.Repositories
{
    public interface IEnemyRepository : 
        IRepositoryGet<Enemy>, 
        IRepositoryUpdate<Enemy>,
        IRepositoryValidate<Enemy>
    {
    }
}
