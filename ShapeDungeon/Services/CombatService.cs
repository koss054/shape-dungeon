using ShapeDungeon.Data;
using ShapeDungeon.DTOs;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Services;
using ShapeDungeon.Repos;

namespace ShapeDungeon.Services
{
    public class CombatService : ICombatService
    {
        private readonly ICombatRepository _combatRepository;
        private readonly IEnemyRepository _enemyRepository;
        private readonly IEnemiesRoomsRepository _enemiesRoomsRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CombatService(
            ICombatRepository combatRepository,
            IEnemyRepository enemyRepository,
            IEnemiesRoomsRepository enemiesRoomsRepository,
            IPlayerRepository playerRepository,
            IRoomRepository roomRepository,
            IUnitOfWork unitOfWork)
        {
            _combatRepository = combatRepository;
            _enemyRepository = enemyRepository;
            _enemiesRoomsRepository = enemiesRoomsRepository;
            _playerRepository = playerRepository;
            _roomRepository = roomRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task InitializeCombat()
        {
            var activeRoom = await _roomRepository.GetActiveForMove();
            await IsActiveRoomValidForCombat(activeRoom);

            var activePlayer = await _playerRepository.GetActive();
            if (activePlayer == null) throw new ArgumentNullException(nameof(activePlayer));

            var activeEnemy = await _enemyRepository.GetActiveForCombat();
            if (activeEnemy == null) throw new ArgumentNullException(nameof(activeEnemy));

            await _unitOfWork.Commit(async () =>
            {
                await _combatRepository.CreateCombat(activePlayer, activeEnemy, activeRoom.Id);
            });
        }

        public async Task<CombatDto> GetActiveCombat()
        {
            var isThereActiveCombat = await _combatRepository.IsActiveCombatPresent();
            if (!isThereActiveCombat) await InitializeCombat();

            var activeCombat = await _combatRepository.GetActiveCombat();
            if (activeCombat == null) throw new ArgumentNullException(nameof(activeCombat));

            var activePlayer = await _playerRepository.GetActive();
            if (activePlayer == null) throw new ArgumentNullException(nameof(activeCombat));

            var activeEnemy = await _enemyRepository.GetActiveForCombat();
            if (activeEnemy == null) throw new ArgumentNullException(nameof(activeCombat));

            activeCombat.Player = activePlayer;
            activeCombat.Enemy = activeEnemy;
            CombatDto combatDto = activeCombat;

            return combatDto;
        }

        public async Task Test(int hp)
        {
            var test = await _combatRepository.GetActiveCombat();
            await _unitOfWork.Commit(() =>
            {
                test!.CurrentEnemyHp = hp;
            });
        }

        public async Task<int> Test2()
        {
            var test = await _combatRepository.GetActiveCombat();
            return test!.CurrentEnemyHp;
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
