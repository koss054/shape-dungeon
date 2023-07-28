using ShapeDungeon.DTOs.Enemies;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Specifications.EnemiesRooms;

namespace ShapeDungeon.Services.Rooms
{
    public class RoomEnemyService : IRoomEnemyService
    {
        private readonly IEnemyRoomRepository _enemyRoomRepository;

        public RoomEnemyService(IEnemyRoomRepository enemyRoomRepository)
        {
            _enemyRoomRepository = enemyRoomRepository;
        }

        public async Task<EnemyDto> GetEnemy(Guid roomId)
        {
            var enemyRoom = await _enemyRoomRepository.GetFirstAsync(
                new EnemyRoomIdSpecification(roomId));

            EnemyDto enemyDto = enemyRoom.Enemy;
            return enemyDto;
        }

        public async Task<bool> IsEnemyDefeated(Guid roomId)
        {
            var enemyRoom = await _enemyRoomRepository.GetFirstAsync(
                new EnemyRoomIdSpecification(roomId));

            return enemyRoom.IsEnemyDefeated;
        }
    }
}
