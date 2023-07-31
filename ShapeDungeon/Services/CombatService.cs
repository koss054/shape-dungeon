using ShapeDungeon.Data;
using ShapeDungeon.DTOs;
using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services;
using ShapeDungeon.Specifications.Combats;
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
        private readonly IPlayerRepository _playerRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IEnemyRoomRepository _enemyRoomRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CombatService(
            ICombatRepository combatRepository,
            IEnemyRepository enemyRepository,
            IPlayerRepository playerRepository,
            IRoomRepository roomRepository,
            IEnemyRoomRepository enemyRoomRepository,
            IUnitOfWork unitOfWork)
        {
            _combatRepository = combatRepository;
            _enemyRepository = enemyRepository;
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

                var combatToAdd = CreateCombat(activePlayer, activeEnemy, activeRoom.Id);

                await _unitOfWork.Commit(async () =>
                {
                    await _combatRepository.AddAsync(combatToAdd);
                });
            }
        }

        public async Task<CombatDto> GetActiveCombat()
        {
            var isThereActiveCombat = await _combatRepository.IsValidByAsync(
                new CombatIsActiveSpecification());

            if (!isThereActiveCombat) await InitializeCombat();

            var activeCombat = await _combatRepository.GetFirstAsync(
                new CombatIsActiveSpecification());

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
            var activeCombat = await _combatRepository.GetFirstAsync(
                new CombatIsActiveSpecification());

            var enemyRoom = await _enemyRoomRepository.GetFirstAsync(
                new EnemyRoomIdSpecification(activeCombat.CombatRoomId));

            activeCombat.IsActive = false;
            activeCombat.Player.IsInCombat = false;

            if (activeCombat.CurrentEnemyHp <= 0)
            {
                enemyRoom.IsEnemyDefeated = true;
                await _unitOfWork.Commit(() =>
                {
                    _enemyRoomRepository.Update(enemyRoom);

                });
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

                await _unitOfWork.Commit(() =>
                {
                    _roomRepository.Update(combatRoom);
                    _roomRepository.Update(prevRoom);
                    _roomRepository.Update(startRoom);
                    _combatRepository.Update(activeCombat);
                });
            }

            return enemyRoom.IsEnemyDefeated;
        }

        public async Task<bool> IsPlayerAttackingInActiveCombat()
        {
            var activeCombat = await _combatRepository.GetFirstAsync(
                new CombatIsActiveSpecification());

            return activeCombat.IsPlayerAttacking;
        }

        public async Task ToggleIsPlayerAttackingInActiveCombat()
        {
            var activeCombat = await _combatRepository.GetFirstAsync(
                new CombatIsActiveSpecification());

            activeCombat.IsPlayerAttacking = !activeCombat.IsPlayerAttacking;

            await _unitOfWork.Commit(() => 
            {
                _combatRepository.Update(activeCombat);
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
            var activeCombat = await _combatRepository.GetFirstAsync(
                new CombatIsActiveSpecification());

            switch (characterType)
            {
                case (int)CombatCharacterType.Player:
                    activeCombat.CurrentPlayerHp -= hpToReduce; break;
                case (int)CombatCharacterType.Enemy:
                    activeCombat.CurrentEnemyHp -= hpToReduce;  break;
                default: throw new ArgumentOutOfRangeException(nameof(characterType));
            }

            await _unitOfWork.Commit(() =>
            {
                _combatRepository.Update(activeCombat);
            });

            return characterType == (int)CombatCharacterType.Player
                ? activeCombat.CurrentPlayerHp
                : activeCombat.CurrentEnemyHp;
        }

        // Exception with string in () will be the name of the custom exception.
        private async Task<bool> IsActiveRoomValidForCombat(Room activeRoom)
        {
            if (!activeRoom.IsEnemyRoom) 
                throw new Exception("NotEnemyRoomException");

            bool isEnemyDefeated = await _enemyRoomRepository.IsValidByAsync(
                new EnemyRoomDefeatedSpecification(activeRoom.Id));

            if (isEnemyDefeated)
                return false;

            return true;
        }

        private Combat CreateCombat(Player player, Enemy enemy, Guid roomId)
        {
            var combat = new Combat
            {
                IsActive = true,
                IsPlayerAttacking = player.Agility >= enemy.Agility,
                PlayerId = player.Id,
                Player = player,
                CurrentPlayerHp = player.Vigor * 2,
                TotalPlayerHp = player.Vigor * 2,
                EnemyId = enemy.Id,
                Enemy = enemy,
                CurrentEnemyHp = enemy.CurrentHp,
                TotalEnemyHp = enemy.CurrentHp,
                CombatRoomId = roomId,
            };

            return combat;
        }
    }
}
