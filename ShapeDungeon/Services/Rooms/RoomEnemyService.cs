using ShapeDungeon.DTOs.Enemies;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Specifications.EnemiesRooms;

namespace ShapeDungeon.Services.Rooms
{
    public class RoomEnemyService : IRoomEnemyService
    {
        private readonly IRepositoryGet<EnemyRoom> _enemyRoomGetRepository;

        public RoomEnemyService(IRepositoryGet<EnemyRoom> enemyRoomGetRepository)
        {
            _enemyRoomGetRepository = enemyRoomGetRepository;
        }

        public async Task<EnemyDto> GetEnemy(Guid roomId)
        {
            var enemyRoom = await _enemyRoomGetRepository.GetFirstOrDefaultByAsync(
                new EnemyRoomIdSpecification(roomId));

            EnemyDto enemyDto = enemyRoom.Enemy;
            return enemyDto;
        }

        public async Task<bool> IsEnemyDefeated(Guid roomId)
        {
            var enemyRoom = await _enemyRoomGetRepository.GetFirstOrDefaultByAsync(
                new EnemyRoomIdSpecification(roomId));

            if (enemyRoom == null) throw new ArgumentNullException();
            return enemyRoom.IsEnemyDefeated;
        }
    }
}
