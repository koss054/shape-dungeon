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
        private readonly IRepositoryCoordsGet<Room> _roomCoordsGetRepository;
        private readonly IRepositoryUpdate<Room> _roomUpdateRepository;
        private readonly IRepositoryGet<Room> _roomRepositoryGet;
        private readonly IUnitOfWork _unitOfWork;

        public RoomCreateService(
            IRepositoryCoordsGet<Room> roomCoordsGetRepository,
            IRepositoryUpdate<Room> roomUpdateRepository,
            IRepositoryGet<Room> roomRepositoryGet,
            IUnitOfWork unitOfWork)
        {
            _roomCoordsGetRepository = roomCoordsGetRepository;
            _roomUpdateRepository = roomUpdateRepository;
            _roomRepositoryGet = roomRepositoryGet;
            _unitOfWork = unitOfWork;
        }

        public async Task<Room> CreateAsync(RoomDetailsDto roomDto)
        {
            Room room = new()
            {
                IsActiveForMove = false,
                IsActiveForScout = false,
                IsActiveForEdit = roomDto.IsStartRoom,
                CanGoLeft = roomDto.CanGoLeft,
                CanGoRight = roomDto.CanGoRight,
                CanGoUp = roomDto.CanGoUp,
                CanGoDown = roomDto.CanGoDown,
                IsStartRoom = roomDto.IsStartRoom,
                IsSafeRoom = roomDto.IsSafeRoom,
                IsEnemyRoom = roomDto.IsEnemyRoom,
                IsEndRoom = roomDto.IsEndRoom,
                CoordX = roomDto.CoordX,
                CoordY = roomDto.CoordY,
            };

            if (room.IsStartRoom)
            {
                room.CoordX = 0;
                room.CoordY = 0;
            }

            await _unitOfWork.Commit(() =>
            {
                _roomUpdateRepository.AddAsync(room);
            });

            return room;
        }

        // There's always going to be an IsActiveForEditRoom == true.
        public async Task<RoomDetailsDto> InitializeRoomAsync(RoomDirection roomDirection)
        {
            var roomDto = new RoomDetailsDto();

            int coordX = await _roomCoordsGetRepository.GetCoordXByAsync(
                new RoomEditSpecification());

            int coordY = await _roomCoordsGetRepository.GetCoordYByAsync(
                new RoomEditSpecification());

            switch (roomDirection)
            {
                case RoomDirection.Left: coordX--; roomDto.CanGoRight = true; break;
                case RoomDirection.Right: coordX++; roomDto.CanGoLeft = true;  break;
                case RoomDirection.Top: coordY++; roomDto.CanGoDown = true; break;
                case RoomDirection.Bottom: coordY--; roomDto.CanGoUp = true; break;
                default: throw new ArgumentOutOfRangeException(nameof(roomDirection));
            }

            roomDto.CoordX = coordX;
            roomDto.CoordY = coordY;
            return roomDto;
        }

        public async Task<bool> AreCoordsInUse(int coordX, int coordY)
            => await _roomRepositoryGet.GetFirstOrDefaultByAsync(
                new RoomCoordsSpecification(coordX, coordY)) != null;
    }
}
