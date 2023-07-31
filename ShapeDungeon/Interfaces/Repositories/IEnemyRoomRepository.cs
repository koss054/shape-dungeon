using ShapeDungeon.Entities;

namespace ShapeDungeon.Interfaces.Repositories
{
    public interface IEnemyRoomRepository :
        IRepositoryGet<EnemyRoom>,
        IRepositoryUpdate<EnemyRoom>,
        IRepositoryValidate<EnemyRoom>
    {
    }
}
