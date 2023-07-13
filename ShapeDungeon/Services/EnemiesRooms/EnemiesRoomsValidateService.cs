using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.EnemiesRooms;
using ShapeDungeon.Repos;
using ShapeDungeon.Specifications.EnemiesRooms;

namespace ShapeDungeon.Services.EnemiesRooms
{
    public class EnemiesRoomsValidateService : IEnemiesRoomsValidateService
    {
        private readonly IRepositoryGet<EnemyRoom> _enemyRoomGetRepository;

        public EnemiesRoomsValidateService(IRepositoryGet<EnemyRoom> enemyRoomGetRepository)
        {
            _enemyRoomGetRepository = enemyRoomGetRepository;
        }

        public async Task<bool> IsRoomEnemyDefeated(Guid roomId)
        {
            var enemyRoom = await _enemyRoomGetRepository.GetFirstOrDefaultByAsync(
                new EnemyRoomIdSpecification(roomId));

            return enemyRoom != null ? enemyRoom.IsEnemyDefeated : true;
        }
    }
}
