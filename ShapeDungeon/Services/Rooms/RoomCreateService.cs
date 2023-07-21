using ShapeDungeon.Data;
using ShapeDungeon.DTOs.Rooms;
using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Specifications.Rooms;
using ShapeDungeon.Strategies.Creational;
using ShapeDungeon.Strategies.Updates;
using ShapeDungeon.Strategies.Updates.Rooms;

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

        public async Task<Room> CreateAsync(RoomDetailsDto roomDto)
        {
            var roomCreateContext = new CreateContext<Room, RoomDetailsDto>(
                new RoomCreateStrategy(roomDto));

            var room = roomCreateContext.ExecuteStrategy();

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

            var roomUpdateContext = new UpdateContext<RoomDetailsDto>(
                new RoomDirectionUpdateStrategy(coordX, coordY, roomDirection));

            return roomUpdateContext.ExecuteStrategy();
        }

        public async Task<bool> AreCoordsInUse(int coordX, int coordY)
            => await _roomRepository.DoCoordsExistByAsync(
                new RoomCoordsSpecification(coordX, coordY));
    }
}
