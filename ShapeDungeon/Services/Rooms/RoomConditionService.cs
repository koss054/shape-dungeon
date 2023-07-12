using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Repos;

namespace ShapeDungeon.Services.Rooms
{
    public class RoomConditionService : IRoomConditionService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IEnemiesRoomsRepository _enemiesRoomsRepository;

        public RoomConditionService(
            IRoomRepository roomRepository,
            IEnemiesRoomsRepository enemiesRoomsRepository)
        {
            _roomRepository = roomRepository;
            _enemiesRoomsRepository = enemiesRoomsRepository;
        }

        /// <summary>
        /// The app needs to know if it has to show the combat screen.
        /// If the player goes to an enemy room and the enemy hasn't been defeated, combat starts.
        /// </summary>
        /// <returns>True, if enemy room has an active enemy. Otherwise, false.</returns>
        public async Task<bool> IsCurrentRoomActiveEnemyRoom()
        {
            var currRoom = await _roomRepository.GetActiveForMove();
            var isEnemyDefeated = await _enemiesRoomsRepository.IsRoomEnemyDefeated(currRoom.Id);

            if (currRoom.IsEnemyRoom && !isEnemyDefeated) return true;
            return false;
        }
    }
}
