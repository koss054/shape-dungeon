using ShapeDungeon.Data;
using ShapeDungeon.DTOs;
using ShapeDungeon.Entities;
using ShapeDungeon.Exceptions;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services;
using ShapeDungeon.Repos;
using ShapeDungeon.Specifications.Enemies;
using ShapeDungeon.Specifications.EnemiesRooms;
using ShapeDungeon.Specifications.Players;
using ShapeDungeon.Specifications.Rooms;

namespace ShapeDungeon.Services
{
    public class CombatService : ICombatService
    {
        private readonly ICombatRepository _combatRepository;
        private readonly IEnemyRepository _enemyRepository;
        private readonly IEnemiesRoomsRepository _enemiesRoomsRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IEnemyRoomRepository _enemyRoomRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CombatService(
            ICombatRepository combatRepository,
            IEnemyRepository enemyRepository,
            IEnemiesRoomsRepository enemiesRoomsRepository,
            IPlayerRepository playerRepository,
            IUnitOfWork unitOfWork,
            IEnemyRoomRepository enemyRoomRepository,
            IRoomRepository roomRepository)
        {
            _combatRepository = combatRepository;
            _enemyRepository = enemyRepository;
            _enemiesRoomsRepository = enemiesRoomsRepository;
            _playerRepository = playerRepository;
            _roomRepository = roomRepository;
            _enemyRoomRepository = enemyRoomRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task InitializeCombat()
        {
            var activeRoom = await _roomRepository.GetFirstAsync(
                new RoomMoveSpecification());

            var isActiveRoomValid = await IsActiveRoomValidForCombat(activeRoom);

            if (isActiveRoomValid)
            {
                var activePlayer = await _playerRepository.GetFirstAsync(
                    new PlayerIsActiveSpecification());

                var activeEnemy = await _enemyRepository.GetFirstAsync(
                    new EnemyActiveForCombatSpecification());

                await _unitOfWork.Commit(async () =>
                {
                    await _combatRepository.CreateCombat(activePlayer, activeEnemy, activeRoom.Id);
                });
            }
        }

        public async Task<CombatDto> GetActiveCombat()
        {
            var isThereActiveCombat = await _combatRepository.IsActiveCombatPresent();
            if (!isThereActiveCombat) await InitializeCombat();

            var activeCombat = await _combatRepository.GetActiveCombat();
            if (activeCombat == null) throw new NoActiveCombatException("bruh");

            var activePlayer = await _playerRepository.GetFirstAsync(
                new PlayerIsActiveSpecification());

            var activeEnemy = await _enemyRepository.GetFirstAsync(
                    new EnemyActiveForCombatSpecification());

            activeCombat.Player = activePlayer;
            activeCombat.Enemy = activeEnemy;
            CombatDto combatDto = activeCombat;

            return combatDto;
        }

        // TODO: Split method so it doesn't do two things at the same time.
        // 1. Updates entity; 2. Returns value.
        // TODO2: Debug here cuz you somehow bugged the player win, lol.
        public async Task<bool> HasPlayerWon()
        {
            var activeCombat = await _combatRepository.GetActiveCombat();
            if (activeCombat == null) throw new ArgumentNullException(
                "IsActive", "NoActiveCombatException");

            var enemyRoom = await _enemiesRoomsRepository
                .GetEntityByRoomId(activeCombat.CombatRoomId);

            await _unitOfWork.Commit(async () =>
            {
                activeCombat.IsActive = false;
                activeCombat.Player.IsInCombat = false;

                if (activeCombat.CurrentEnemyHp <= 0)
                {
                    enemyRoom.IsEnemyDefeated = true;
                }
                else
                {
                    var combatRoom = await _roomRepository.GetFirstAsync(
                        new RoomMoveSpecification());

                    var prevRoom = await _roomRepository.GetFirstAsync(
                        new RoomScoutSpecification());

                    var startRoom = await _roomRepository.GetFirstAsync(
                        new RoomCoordsSpecification(0, 0));

                    combatRoom.IsActiveForScout = false;
                    combatRoom.IsActiveForMove = false;
                    prevRoom.IsActiveForScout = false;
                    prevRoom.IsActiveForMove = false;

                    // Start room always exists - seeded to db with coords x0 y0.
                    startRoom!.IsActiveForScout = true;
                    startRoom!.IsActiveForMove = true;

                    activeCombat.Player.CurrentExp = 0;
                }
            });

            return enemyRoom.IsEnemyDefeated;
        }

        public async Task<bool> IsPlayerAttackingInActiveCombat()
        {
            var activeCombat = await _combatRepository.GetActiveCombat();
            if (activeCombat == null) throw new ArgumentNullException(
                "IsActive", "NoActiveCombatException");

            return activeCombat.IsPlayerAttacking;
        }

        public async Task ToggleIsPlayerAttackingInActiveCombat()
        {
            var activeCombat = await _combatRepository.GetActiveCombat();
            if (activeCombat == null) throw new ArgumentNullException(
                "IsActive", "NoActiveCombatException");

            await _unitOfWork.Commit(() => 
            {
                activeCombat.IsPlayerAttacking = !activeCombat.IsPlayerAttacking;
            });
        }

        /// <summary>
        /// Updates the health of the character that has been attacked.
        /// </summary>
        /// <param name="hpToReduce">The amount of hp the attacked character has lost.</param>
        /// <param name="characterType"> 
        /// CombatCharacterType.Player == 0. CombatCharacterType.Enemy == 1.
        /// Any other int value will throw an exception.
        /// </param>
        /// <returns>The updated health value of the affected character type.</returns>
        /// <exception cref="ArgumentNullException">No active combat could be found.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Incorrect character type has been passed.</exception>
        public async Task<int> UpdateHealthAfterAttack(int hpToReduce, int characterType)
        {
            var activeCombat = await _combatRepository.GetActiveCombat();
            if (activeCombat == null) throw new ArgumentNullException(
                "IsActive", "NoActiveCombatException");

            await _unitOfWork.Commit(() =>
            {
                switch (characterType)
                {
                    case (int)CombatCharacterType.Player:
                        activeCombat.CurrentPlayerHp -= hpToReduce; break;
                    case (int)CombatCharacterType.Enemy:
                        activeCombat.CurrentEnemyHp -= hpToReduce;  break;
                    default: throw new ArgumentOutOfRangeException(nameof(characterType));
                }
            });

            return characterType == (int)CombatCharacterType.Player
                ? activeCombat.CurrentPlayerHp
                : activeCombat.CurrentEnemyHp;
        }

        // Exception with string in () will be the name of the custom exception.
        private async Task<bool> IsActiveRoomValidForCombat(Room activeRoom)
        {
            if (activeRoom == null) 
                throw new ArgumentNullException(nameof(activeRoom));

            if (!activeRoom.IsEnemyRoom) 
                throw new Exception("NotEnemyRoomException");

            bool isEnemyDefeated = await _enemyRoomRepository.IsValidByAsync(
                new EnemyRoomDefeatedSpecification(activeRoom.Id));

            if (isEnemyDefeated)
                return false;

            return true;
        }
    }
}
