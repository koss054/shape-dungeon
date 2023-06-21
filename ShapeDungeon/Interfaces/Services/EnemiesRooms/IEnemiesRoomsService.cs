using ShapeDungeon.Entities;

namespace ShapeDungeon.Interfaces.Services.EnemiesRooms
{
    public interface IEnemiesRoomsService
    {
        Task AddEnemyToRoom(Room room, Enemy enemy);
    }
}
