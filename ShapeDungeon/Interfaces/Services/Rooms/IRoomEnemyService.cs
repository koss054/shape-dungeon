using ShapeDungeon.DTOs.Enemies;

namespace ShapeDungeon.Interfaces.Services.Rooms
{
    public interface IRoomEnemyService
    {
        Task<EnemyDto> GetEnemy(Guid roomId);
        Task<bool> IsEnemyDefeated(Guid roomId);
    }
}
