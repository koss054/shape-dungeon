using ShapeDungeon.Data;
using ShapeDungeon.DTOs;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Services;
using ShapeDungeon.Repos;

namespace ShapeDungeon.Services
{
    public class CombatService : ICombatService
    {
        private readonly IEnemyRepository _enemyRepository;
        private readonly IEnemiesRoomsRepository _enemiesRoomsRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CombatService(
            IEnemyRepository enemyRepository,
            IEnemiesRoomsRepository enemiesRoomsRepository,
            IPlayerRepository playerRepository,
            IRoomRepository roomRepository,
            IUnitOfWork unitOfWork)
        {
            _enemyRepository = enemyRepository;
            _enemiesRoomsRepository = enemiesRoomsRepository;
            _playerRepository = playerRepository;
            _roomRepository = roomRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CombatDto> InitializeCombat()
        {
            var activeRoom = await _roomRepository.GetActiveForMove();
            await IsActiveRoomValidForCombat(activeRoom);

            var activePlayer = await _playerRepository.GetActive();
            if (activePlayer == null) throw new ArgumentNullException(nameof(activePlayer));

            var activeEnemy = await _enemyRepository.GetActiveForCombat();
            if (activeEnemy == null) throw new ArgumentNullException(nameof(activeEnemy));

            var combatDto = new CombatDto()
            {
                Player = activePlayer,
                Enemy = activeEnemy,
                CombatRoomId = activeRoom.Id,
            };

            return combatDto;
        }

        // Exception with string in () will be the name of the custom exception.
        private async Task IsActiveRoomValidForCombat(Room activeRoom)
        {
            if (activeRoom == null) 
                throw new ArgumentNullException(nameof(activeRoom));

            if (!activeRoom.IsEnemyRoom) 
                throw new Exception("NotEnemyRoomException");

            bool isEnemyDefeated = await _enemiesRoomsRepository
                .IsRoomEnemyDefeated(activeRoom.Id);

            if (isEnemyDefeated)
                throw new Exception("EnemyAlreadyDefeatedException");
        }
    }
}
