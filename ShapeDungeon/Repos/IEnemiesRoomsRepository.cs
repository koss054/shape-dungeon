using ShapeDungeon.Entities;

namespace ShapeDungeon.Repos
{
    public interface IEnemiesRoomsRepository
    {
        Task AddAsync(Room room, Enemy enemy);
        Task<Enemy> GetEnemyByRoomId(Guid roomId);
        Task<Guid> GetEnemyIdByRoomId(Guid roomId);
        Task DefeatEnemyForRoom(Guid roomId);

        /// <summary>
        /// Checks if the enemy in the searched room has been defeated.
        /// </summary>
        /// <param name="roomId">Id of the room in which the enemy status is being checked.</param>
        /// <returns>
        /// If enemy room exists, the value of the IsEnemyDefeated property.
        /// If enemy room doesn't exist, true (dude's been evaporated from existance, lol).
        /// </returns>
        Task<bool> IsRoomEnemyDefeated(Guid roomId);
    }
}
