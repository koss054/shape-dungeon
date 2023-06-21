using ShapeDungeon.Entities;

namespace ShapeDungeon.Repos
{
    public interface IEnemiesRoomsRepository
    {
        Task AddAsync(Room room, Enemy enemy);
        Task<Enemy> GetEnemyByRoomId(Guid roomId);
    }
}
