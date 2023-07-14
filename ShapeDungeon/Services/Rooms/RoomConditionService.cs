using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.EnemiesRooms;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Specifications.Rooms;

namespace ShapeDungeon.Services.Rooms
{
    public class RoomConditionService : IRoomConditionService
    {
        private readonly IRepositoryGet<Room> _roomGetRepository;
        private readonly IEnemiesRoomsValidateService _enemiesRoomsValidateService;

        public RoomConditionService(
            IRepositoryGet<Room> roomGetRepository,
            IEnemiesRoomsValidateService enemiesRoomsValidateService)
        {
            _roomGetRepository = roomGetRepository;
            _enemiesRoomsValidateService = enemiesRoomsValidateService;
        }

        /// <summary>
        /// The app needs to know if it has to show the combat screen.
        /// If the player goes to an enemy room and the enemy hasn't been defeated, combat starts.
        /// </summary>
        /// <returns>True, if enemy room has an active enemy. Otherwise, false.</returns>
        public async Task<bool> IsCurrentRoomActiveEnemyRoom()
        {
            var currRoom = await _roomGetRepository.GetFirstOrDefaultByAsync(
                new RoomMoveSpecification());

            var isEnemyDefeated = await _enemiesRoomsValidateService
                .IsRoomEnemyDefeated(currRoom.Id);

            if (currRoom.IsEnemyRoom && !isEnemyDefeated) 
                return true;

            return false;
        }
    }
}
