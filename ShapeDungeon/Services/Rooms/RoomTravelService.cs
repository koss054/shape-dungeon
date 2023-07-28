using ShapeDungeon.Data;
using ShapeDungeon.DTOs.Rooms;
using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Specifications.Enemies;
using ShapeDungeon.Specifications.EnemiesRooms;
using ShapeDungeon.Specifications.Rooms;

namespace ShapeDungeon.Services.Rooms
{
    public class RoomTravelService : IRoomTravelService
    {
        private readonly IEnemyRoomRepository _enemyRoomRepository;
        private readonly IRoomValidateService _roomValidateService;
        private readonly IRoomRepository _roomRepository;
        private readonly IEnemyRepository _enemyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RoomTravelService(
            IEnemyRoomRepository enemyRoomRepository,
            IRoomValidateService roomValidateService,
            IRoomRepository roomRepository,
            IEnemyRepository enemyRepository,
            IUnitOfWork unitOfWork)
        {
            _enemyRoomRepository = enemyRoomRepository;
            _roomValidateService = roomValidateService;
            _roomRepository = roomRepository;
            _enemyRepository = enemyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task RoomTravelAsync(RoomDirection direction, RoomTravelAction action)
        {
            Room oldRoom = await GetOldRoom(action);

            var coordsDto = new RoomCoordsDto(oldRoom.CoordX, oldRoom.CoordY);
            var areCoordsUpdated = await AreCoordsUpdatedDto(direction, oldRoom, coordsDto);

            if (!areCoordsUpdated)
                return;

            var newRoom = await _roomRepository.GetFirstAsync(
                new RoomCoordsSpecification(coordsDto.CoordX, coordsDto.CoordY));

            ToggleRoomIsActiveProperties(oldRoom, newRoom, action);

            var oldEnemy = await ClearOldEnemyActiveForCombat();
            var newEnemy = await ActivateNewEnemyForCombat(newRoom);

            await _unitOfWork.Commit(() =>
            {
                _roomRepository.Update(oldRoom);
                _roomRepository.Update(newRoom);

                if (action == RoomTravelAction.Move 
                    && oldEnemy is not null 
                    && newEnemy is not null)
                {
                    _enemyRepository.Update(oldEnemy);
                    _enemyRepository.Update(newEnemy);
                }
            });
        }

        public async Task<bool> IsScoutResetAsync()
        {
            var activeForScoutRoom = await _roomRepository.GetFirstAsync(
                new RoomScoutSpecification());

            var activeForMoveRoom = await _roomRepository.GetFirstAsync(
                new RoomMoveSpecification());

            if (activeForScoutRoom != null && activeForMoveRoom != null)
            {
                activeForScoutRoom.IsActiveForScout = false;
                activeForMoveRoom.IsActiveForScout = true;

                await _unitOfWork.Commit(() =>
                {
                    _roomRepository.Update(activeForScoutRoom);
                    _roomRepository.Update(activeForMoveRoom);
                });

                return true;
            }

            return false;
        }

        private async Task<Room> GetOldRoom(RoomTravelAction action)
        {
            return action switch
            {
                RoomTravelAction.Move => await _roomRepository.GetFirstAsync(
                    new RoomMoveSpecification()),
                RoomTravelAction.Scout => await _roomRepository.GetFirstAsync(
                    new RoomScoutSpecification()),
                _ => throw new ArgumentOutOfRangeException(nameof(action)),
            };
        }

        private async Task<bool> AreCoordsUpdatedDto(RoomDirection direction, Room oldRoom, RoomCoordsDto coordsDto)
        {
            var areCoordsUpdated = false;

            switch (direction)
            {
                case RoomDirection.Left:
                    var isLeftDeadEnd = !(await _roomValidateService
                        .CanEnterRoomFromDirection(coordsDto.CoordX - 1, coordsDto.CoordY,
                        RoomDirection.Left));

                    if (oldRoom.CanGoLeft && !isLeftDeadEnd)
                    {
                        coordsDto.CoordX--;
                        areCoordsUpdated = true;
                    }

                    break;
                case RoomDirection.Right:
                    var isRightDeadEnd = !(await _roomValidateService
                        .CanEnterRoomFromDirection(coordsDto.CoordX + 1, coordsDto.CoordY,
                        RoomDirection.Right));

                    if (oldRoom.CanGoRight && !isRightDeadEnd)
                    {
                        coordsDto.CoordX++;
                        areCoordsUpdated = true;
                    }

                    break;
                case RoomDirection.Top:
                    var isUpDeadEnd = !(await _roomValidateService
                        .CanEnterRoomFromDirection(coordsDto.CoordX, coordsDto.CoordY + 1,
                        RoomDirection.Top));

                    if (oldRoom.CanGoUp && !isUpDeadEnd)
                    {
                        coordsDto.CoordY++;
                        areCoordsUpdated = true;
                    }

                    break;
                case RoomDirection.Bottom:
                    var isDownDeadEnd = !(await _roomValidateService
                        .CanEnterRoomFromDirection(coordsDto.CoordX, coordsDto.CoordY - 1,
                        RoomDirection.Bottom));

                    if (oldRoom.CanGoDown && !isDownDeadEnd)
                    {
                        coordsDto.CoordY--;
                        areCoordsUpdated = true;
                    }

                    break;
                default: throw new ArgumentOutOfRangeException(nameof(direction));
            }

            return areCoordsUpdated;
        }

        /// <summary>
        /// Gets the currently active for combat enemy 
        /// and changes it's IsActiveForCombat property from true to false.
        /// </summary>
        /// <returns>Returns old active for combat enemy if there was one, otherwise null.</returns>
        private async Task<Enemy?> ClearOldEnemyActiveForCombat()
        {
            var isAlreadyActive = await _enemyRepository.IsValidByAsync(
                new EnemyActiveForCombatSpecification());

            if (!isAlreadyActive)
                return null;

            var oldEnemy = await _enemyRepository.GetFirstAsync(
                new EnemyActiveForCombatSpecification());

            oldEnemy.IsActiveForCombat = false;
            return oldEnemy;
        }

        /// <summary>
        /// Gets the enemy in the room the player is in if the room is an enemy room 
        /// and if the enemy in it hasn't been defeated yet. Sets it's IsActiveForCombat property
        /// to true if the previously mentioned requirements are met.
        /// </summary>
        /// <param name="currRoom">The room that the player is in.</param>
        /// <returns>Returns new active for combat enemy if requirements are met, otherwise null.</returns>
        private async Task<Enemy?> ActivateNewEnemyForCombat(Room currRoom)
        {
            if (!currRoom.IsEnemyRoom)
                return null;

            var enemyRoom = await _enemyRoomRepository.GetFirstAsync(
                    new EnemyRoomIdSpecification(currRoom.Id));

            if (enemyRoom.IsEnemyDefeated)
                return null;

            var newEnemy = enemyRoom.Enemy;
            newEnemy.IsActiveForCombat = true;
            return newEnemy;
        }

        private void ToggleRoomIsActiveProperties(Room oldRoom, Room newRoom, RoomTravelAction action)
        {
            switch (action)
            {
                case RoomTravelAction.Move:
                    oldRoom.IsActiveForMove = false;
                    newRoom.IsActiveForMove = true;
                    break;
                case RoomTravelAction.Scout:
                    oldRoom.IsActiveForScout = false;
                    newRoom.IsActiveForScout = true;
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(action));
            }
        }
    }
}
