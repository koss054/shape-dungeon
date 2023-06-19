using ShapeDungeon.Entities;

namespace ShapeDungeon.Interfaces.Services.RoomsEnemies
{
    public interface IEnemiesRoomsService
    {
        Task AddEnemyToRoom(Room room, Enemy enemy);
    }
}
