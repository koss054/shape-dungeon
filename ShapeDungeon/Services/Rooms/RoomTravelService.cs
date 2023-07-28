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
            await UpdateCoordsDto(direction, oldRoom, coordsDto);

            var newRoom = await _roomRepository.GetFirstAsync(
                new RoomCoordsSpecification(coordsDto.CoordX, coordsDto.CoordY));

            ToggleRoomIsActiveProperties(oldRoom, newRoom, action);

            await _unitOfWork.Commit(async () =>
            {
                _roomRepository.Update(oldRoom);
                _roomRepository.Update(newRoom);

                if (action == RoomTravelAction.Move)
                {
                    await ClearActiveForCombat();
                    await ActivateEnemyForCombat(newRoom);
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

        private async Task UpdateCoordsDto(RoomDirection direction, Room oldRoom, RoomCoordsDto coordsDto)
        {
            switch (direction)
            {
                case RoomDirection.Left:
                    var isLeftDeadEnd = !(await _roomValidateService
                        .CanEnterRoomFromDirection(coordsDto.CoordX - 1, coordsDto.CoordY,
                        RoomDirection.Left));

                    if (oldRoom.CanGoLeft && !isLeftDeadEnd)
                        coordsDto.CoordX--;

                    break;
                case RoomDirection.Right:
                    var isRightDeadEnd = !(await _roomValidateService
                        .CanEnterRoomFromDirection(coordsDto.CoordX + 1, coordsDto.CoordY,
                        RoomDirection.Right));

                    if (oldRoom.CanGoRight && !isRightDeadEnd)
                        coordsDto.CoordX++;

                    break;
                case RoomDirection.Top:
                    var isUpDeadEnd = !(await _roomValidateService
                        .CanEnterRoomFromDirection(coordsDto.CoordX, coordsDto.CoordY + 1,
                        RoomDirection.Top));

                    if (oldRoom.CanGoUp && !isUpDeadEnd)
                        coordsDto.CoordY++;

                    break;
                case RoomDirection.Bottom:
                    var isDownDeadEnd = !(await _roomValidateService
                        .CanEnterRoomFromDirection(coordsDto.CoordX, coordsDto.CoordY - 1,
                        RoomDirection.Bottom));

                    if (oldRoom.CanGoDown && !isDownDeadEnd)
                        coordsDto.CoordY--;

                    break;
                default: throw new ArgumentOutOfRangeException(nameof(direction));
            }
        }

        /// <summary>
        /// Use only in Unit of Work block.
        /// </summary>
        /// <returns>No return value - removes active for combat enemy.</returns>
        private async Task ClearActiveForCombat()
        {
            var isAlreadyActive = await _enemyRepository.IsValidByAsync(
                new EnemyActiveForCombatSpecification());

            if (isAlreadyActive)
            {
                var oldEnemy = await _enemyRepository.GetFirstAsync(
                    new EnemyActiveForCombatSpecification());

                oldEnemy.IsActiveForCombat = false;
                _enemyRepository.Update(oldEnemy);
            }
        }

        /// <summary>
        /// Use only in Unit of Work block.
        /// </summary>
        /// <param name="currRoom">The room that the player is in.</param>
        /// <returns>No return value - adds active for combat enemy if combat room and enemy undefeated.</returns>
        private async Task ActivateEnemyForCombat(Room currRoom) 
        {
            if (currRoom.IsEnemyRoom)
            {
                var enemyRoom = await _enemyRoomRepository.GetFirstAsync(
                        new EnemyRoomIdSpecification(currRoom.Id));

                if (!enemyRoom.IsEnemyDefeated)
                {
                    var newEnemy = enemyRoom.Enemy;
                    newEnemy.IsActiveForCombat = true;
                    _enemyRepository.Update(newEnemy);
                }
            }
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
