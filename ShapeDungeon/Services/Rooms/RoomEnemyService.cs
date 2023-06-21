using ShapeDungeon.DTOs.Enemies;
using ShapeDungeon.Interfaces.Services.EnemiesRooms;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Repos;

namespace ShapeDungeon.Services.Rooms
{
    public class RoomEnemyService : IRoomEnemyService
    {
        private readonly IEnemiesRoomsRepository _enemiesRoomsRepository;

        public RoomEnemyService(IEnemiesRoomsRepository enemiesRoomsRepository)
        {
            _enemiesRoomsRepository = enemiesRoomsRepository;
        }

        public async Task<EnemyDto> GetEnemy(Guid roomId)
        {
            var enemy = await _enemiesRoomsRepository.GetEnemyByRoomId(roomId);
            EnemyDto enemyDto = enemy;
            return enemyDto;
        }
    }
}
