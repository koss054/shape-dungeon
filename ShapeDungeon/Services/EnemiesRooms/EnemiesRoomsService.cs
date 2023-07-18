using ShapeDungeon.Data;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.EnemiesRooms;
using ShapeDungeon.Repos;

namespace ShapeDungeon.Services.EnemiesRooms
{
    public class EnemiesRoomsService : IEnemiesRoomsService
    {
        private readonly IEnemyRoomRepository _enemyRoomRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EnemiesRoomsService(
            IEnemyRoomRepository enemyRoomRepository,
            IUnitOfWork unitOfWork)
        {
            _enemyRoomRepository = enemyRoomRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddEnemyToRoom(Room room, Enemy enemy)
        {
            var newEnemyRoom = new EnemyRoom
            {
                EnemyId = enemy.Id,
                Enemy = enemy,
                RoomId = room.Id,
                Room = room,
                IsEnemyDefeated = false,
            };

            await _unitOfWork.Commit(() =>
            {
                _enemyRoomRepository.AddAsync(newEnemyRoom);
            });
        }
    }
}
