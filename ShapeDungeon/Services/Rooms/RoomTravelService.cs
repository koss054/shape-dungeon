﻿using ShapeDungeon.Data;
using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Repos;
using ShapeDungeon.Specifications.EnemiesRooms;
using ShapeDungeon.Specifications.Rooms;

namespace ShapeDungeon.Services.Rooms
{
    public class RoomTravelService : IRoomTravelService
    {
        private readonly IRepositoryGet<EnemyRoom> _enemyRoomGetRepository;
        private readonly IRoomValidateService _roomValidateService;
        private readonly IRepositoryGet<Room> _roomGetRepository;
        private readonly IEnemyRepository _enemyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RoomTravelService(
            IRepositoryGet<EnemyRoom> enemyRoomGetRepository,
            IRoomValidateService roomValidateService,
            IRepositoryGet<Room> roomGetRepository,
            IEnemyRepository enemyRepository,
            IUnitOfWork unitOfWork)
        {
            _enemyRoomGetRepository = enemyRoomGetRepository;
            _roomValidateService = roomValidateService;
            _roomGetRepository = roomGetRepository;
            _enemyRepository = enemyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task RoomTravelAsync(RoomDirection direction, RoomTravelAction action)
        {
            var oldRoom = action switch
            {
                RoomTravelAction.Move => await _roomGetRepository.GetFirstOrDefaultByAsync(
                    new RoomMoveSpecification()),
                RoomTravelAction.Scout => await _roomGetRepository.GetFirstOrDefaultByAsync(
                    new RoomScoutSpecification()),
                _ => throw new ArgumentOutOfRangeException(nameof(action)),
            };

            if (oldRoom == null) 
                throw new ArgumentNullException(nameof(oldRoom));

            var coordX = oldRoom!.CoordX;
            var coordY = oldRoom!.CoordY;

            switch (direction)
            {
                case RoomDirection.Left: 
                    var isLeftDeadEnd = !(await _roomValidateService
                        .CanEnterRoomFromDirection(coordX - 1, coordY, RoomDirection.Left));

                    if (oldRoom.CanGoLeft && !isLeftDeadEnd)
                        coordX--;

                    break;
                case RoomDirection.Right:
                    var isRightDeadEnd = !(await _roomValidateService
                        .CanEnterRoomFromDirection(coordX + 1, coordY, RoomDirection.Right));

                    if (oldRoom.CanGoRight && !isRightDeadEnd) 
                        coordX++;  

                    break;
                case RoomDirection.Top: 
                    var isUpDeadEnd = !(await _roomValidateService
                        .CanEnterRoomFromDirection(coordX, coordY + 1, RoomDirection.Top));

                    if (oldRoom.CanGoUp && !isUpDeadEnd) 
                        coordY++; 

                    break;
                case RoomDirection.Bottom: 
                    var isDownDeadEnd = !(await _roomValidateService
                        .CanEnterRoomFromDirection(coordX, coordY - 1, RoomDirection.Bottom));

                    if (oldRoom.CanGoDown && !isDownDeadEnd)
                        coordY--;  

                    break;
                default: throw new ArgumentOutOfRangeException(nameof(direction));
            }

            var newRoom = await _roomGetRepository.GetFirstOrDefaultByAsync(
                new RoomCoordsSpecification(coordX, coordY));

            if (newRoom != null)
            {
                await _unitOfWork.Commit(async () =>
                {
                    switch (action)
                    {
                        case RoomTravelAction.Move:
                            oldRoom.IsActiveForMove = false;
                            newRoom.IsActiveForMove = true;
                            await _enemyRepository.ClearActiveForCombat();
                            await ActivateEnemyForCombat(newRoom);
                            break;
                        case RoomTravelAction.Scout:
                            oldRoom.IsActiveForScout = false;
                            newRoom.IsActiveForScout = true;
                            break;
                        default: throw new ArgumentOutOfRangeException(nameof(action));
                    }
                });
            }
        }

        public async Task<bool> IsScoutResetAsync()
        {
            var activeForScoutRoom = await _roomGetRepository.GetFirstOrDefaultByAsync(
                new RoomScoutSpecification());

            var activeForMoveRoom = await _roomGetRepository.GetFirstOrDefaultByAsync(
                new RoomMoveSpecification());

            if (activeForScoutRoom != null && activeForMoveRoom != null)
            {
                await _unitOfWork.Commit(() =>
                {
                    activeForScoutRoom.IsActiveForScout = false;
                    activeForMoveRoom.IsActiveForScout = true;
                });

                return true;
            }

            return false;
        }

        private async Task ActivateEnemyForCombat(Room currRoom) 
        {
            if (currRoom.IsEnemyRoom)
            {
                var enemyRoom = await _enemyRoomGetRepository.GetFirstOrDefaultByAsync(
                        new EnemyRoomIdSpecification(currRoom.Id));

                if (enemyRoom.IsEnemyDefeated)
                    await _enemyRepository.SetActiveForCombat(enemyRoom.EnemyId);
            }
        }
    }
}
