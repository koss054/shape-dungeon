using ShapeDungeon.Entities;

namespace ShapeDungeon.Interfaces.Repositories
{
    public interface IRoomRepository :
        IRepositoryGet<Room>,
        IRepositoryUpdate<Room>,
        IRepositoryCoordsGet<Room>
    {
    }
}
