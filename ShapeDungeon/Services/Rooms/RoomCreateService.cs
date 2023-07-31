using ShapeDungeon.Data;
using ShapeDungeon.DTOs.Rooms;
using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Specifications.Rooms;

namespace ShapeDungeon.Services.Rooms
{
    public class RoomCreateService : IRoomCreateService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RoomCreateService(
            IRoomRepository roomRepository,
            IUnitOfWork unitOfWork)
        {
            _roomRepository = roomRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Room> CreateAsync(RoomDetailsDto rDto)
        {
            Room room = new()
            {
                IsActiveForMove = false,
                IsActiveForScout = false,
                IsActiveForEdit = rDto.IsStartRoom,
                CanGoLeft = rDto.CanGoLeft,
                CanGoRight = rDto.CanGoRight,
                CanGoUp = rDto.CanGoUp,
                CanGoDown = rDto.CanGoDown,
                IsStartRoom = rDto.IsStartRoom,
                IsSafeRoom = rDto.IsSafeRoom,
                IsEnemyRoom = rDto.IsEnemyRoom,
                IsEndRoom = rDto.IsEndRoom,
                CoordX = rDto.CoordX,
                CoordY = rDto.CoordY,
            };

            if (room.IsStartRoom)
            {
                room.CoordX = 0;
                room.CoordY = 0;
            }

            await _unitOfWork.Commit(() =>
            {
                _roomRepository.AddAsync(room);
            });

            return room;
        }

        // There's always going to be an IsActiveForEditRoom == true.
        public async Task<RoomDetailsDto> InitializeRoomAsync(RoomDirection roomDirection)
        {
            int coordX = await _roomRepository.GetCoordXByAsync(
                new RoomEditSpecification());

            int coordY = await _roomRepository.GetCoordYByAsync(
                new RoomEditSpecification());

            var rDto = new RoomDetailsDto();

            switch (roomDirection)
            {
                case RoomDirection.Left: coordX--; rDto.CanGoRight = true; break;
                case RoomDirection.Right: coordX++; rDto.CanGoLeft = true; break;
                case RoomDirection.Top: coordY++; rDto.CanGoDown = true; break;
                case RoomDirection.Bottom: coordY--; rDto.CanGoUp = true; break;
                default: throw new ArgumentOutOfRangeException(nameof(roomDirection));
            }

            rDto.CoordX = coordX;
            rDto.CoordY = coordY;
            return rDto;
        }

        public async Task<bool> AreCoordsInUse(int coordX, int coordY)
            => await _roomRepository.DoCoordsExistByAsync(
                new RoomCoordsSpecification(coordX, coordY));
    }
}
