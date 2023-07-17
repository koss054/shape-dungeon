using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Specifications.EnemiesRooms;
using ShapeDungeon.Specifications.Rooms;

namespace ShapeDungeon.Services.Rooms
{
    public class RoomConditionService : IRoomConditionService
    {
        private readonly IRepositoryGet<Room> _roomGetRepository;
        private readonly IRepositoryValidate<EnemyRoom> _enemyRoomValidateRepository;

        public RoomConditionService(
            IRepositoryGet<Room> roomGetRepository, 
            IRepositoryValidate<EnemyRoom> enemyRoomValidateRepository)
        {
            _roomGetRepository = roomGetRepository;
            _enemyRoomValidateRepository = enemyRoomValidateRepository;
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

            var isEnemyDefeated = await _enemyRoomValidateRepository.IsValidByAsync(
                new EnemyRoomDefeatedSpecification());

            if (currRoom.IsEnemyRoom && !isEnemyDefeated) 
                return true;

            return false;
        }
    }
}
