using ShapeDungeon.Data;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Services.EnemiesRooms;
using ShapeDungeon.Repos;

namespace ShapeDungeon.Services.RoomsEnemies
{
    public class EnemiesRoomsService : IEnemiesRoomsService
    {
        private readonly IEnemiesRoomsRepository _enemiesRoomsRepository;
        private readonly IUnitOfWork _unitOfWork;

        public EnemiesRoomsService(
            IEnemiesRoomsRepository enemiesRoomsRepository, 
            IUnitOfWork unitOfWork)
        {
            _enemiesRoomsRepository = enemiesRoomsRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddEnemyToRoom(Room room, Enemy enemy)
        {
            await _unitOfWork.Commit(() =>
            {
                _enemiesRoomsRepository.AddAsync(room, enemy);
            });
        }
    }
}
