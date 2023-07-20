using ShapeDungeon.Data;
using ShapeDungeon.DTOs.EnemiesRooms;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.EnemiesRooms;
using ShapeDungeon.Strategies.Creational;

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
            var enemyRoomCreateContext = new CreateContext<EnemyRoom, EnemyRoomDto>(
                new EnemyRoomCreateStrategy(new EnemyRoomDto(enemy, room)));

            var enemyRoom = enemyRoomCreateContext.ExecuteStrategy();

            await _unitOfWork.Commit(() =>
            {
                _enemyRoomRepository.AddAsync(enemyRoom);
            });
        }
    }
}
