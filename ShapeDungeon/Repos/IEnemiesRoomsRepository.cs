using ShapeDungeon.Entities;

namespace ShapeDungeon.Repos
{
    public interface IEnemiesRoomsRepository
    {
        Task AddAsync(Room room, Enemy enemy);
        Task<Enemy> GetEnemyByRoomId(Guid roomId);
        Task<Guid> GetEnemyIdByRoomId(Guid roomId);
        Task<EnemyRoom> GetEntityByRoomId(Guid roomId);
    }
}
