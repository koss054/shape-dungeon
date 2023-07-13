using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Repos;
using ShapeDungeon.Specifications.Rooms;

namespace ShapeDungeon.Services.Rooms
{
    public class RoomConditionService : IRoomConditionService
    {
        private readonly IRepositoryGet<Room> _roomGetRepository;
        private readonly IEnemiesRoomsRepository _enemiesRoomsRepository;

        public RoomConditionService(
            IRepositoryGet<Room> roomGetRepository,
            IEnemiesRoomsRepository enemiesRoomsRepository)
        {
            _roomGetRepository = roomGetRepository;
            _enemiesRoomsRepository = enemiesRoomsRepository;
        }

        /// <summary>
        /// The app needs to know if it has to show the combat screen.
        /// If the player goes to an enemy room and the enemy hasn't been defeated, combat starts.
        /// </summary>
        /// <returns>True, if enemy room has an active enemy. Otherwise, false.</returns>
        public async Task<bool> IsCurrentRoomActiveEnemyRoom()
        {
            var currRoom = await _roomGetRepository.GetFirstOrDefaultByAsync(
                new RoomTypeSpecification(RoomActiveType.Move));

            var isEnemyDefeated = await _enemiesRoomsRepository
                .IsRoomEnemyDefeated(currRoom.Id);

            if (currRoom.IsEnemyRoom && !isEnemyDefeated) 
                return true;

            return false;
        }
    }
}
