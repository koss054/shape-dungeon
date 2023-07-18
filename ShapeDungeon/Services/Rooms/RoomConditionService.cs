using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Specifications.EnemiesRooms;
using ShapeDungeon.Specifications.Rooms;

namespace ShapeDungeon.Services.Rooms
{
    public class RoomConditionService : IRoomConditionService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IRepositoryValidate<EnemyRoom> _enemyRoomValidateRepository;

        public RoomConditionService(
            IRoomRepository roomRepository, 
            IRepositoryValidate<EnemyRoom> enemyRoomValidateRepository)
        {
            _roomRepository = roomRepository;
            _enemyRoomValidateRepository = enemyRoomValidateRepository;
        }

        /// <summary>
        /// The app needs to know if it has to show the combat screen.
        /// If the player goes to an enemy room and the enemy hasn't been defeated, combat starts.
        /// </summary>
        /// <returns>True, if enemy room has an active enemy. Otherwise, false.</returns>
        public async Task<bool> IsCurrentRoomActiveEnemyRoom()
        {
            var currRoom = await _roomRepository.GetFirstAsync(
                new RoomMoveSpecification());

            var isEnemyDefeated = await _enemyRoomValidateRepository.IsValidByAsync(
                new EnemyRoomDefeatedSpecification(currRoom.Id));

            if (currRoom.IsEnemyRoom && !isEnemyDefeated) 
                return true;

            return false;
        }
    }
}
