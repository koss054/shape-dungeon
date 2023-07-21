using ShapeDungeon.DTOs.Rooms;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Specifications.Rooms;
using ShapeDungeon.Strategies.Updates;
using ShapeDungeon.Strategies.Updates.Rooms;

namespace ShapeDungeon.Services.Rooms
{
    public class CheckRoomNeighborsService : ICheckRoomNeighborsService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IRoomValidateService _roomValidateService;

        public CheckRoomNeighborsService(
            IRoomRepository roomRepository,
            IRoomValidateService roomValidateService)
        {
            _roomRepository = roomRepository;
            _roomValidateService = roomValidateService;
        }

        public async Task<RoomNavDto?> SetDtoNeighborsAsync(int coordX, int coordY)
        {
            var currRoom = await InitializeCheckRoomAsync(coordX, coordY);
            var roomUpdateAsyncContext = new UpdateAsyncContext<RoomNavDto>(
                new RoomSetNeighborsStrategy(
                    IsRoomWithCoordsValidAsync(coordX - 1, coordY),
                    IsRoomWithCoordsValidAsync(coordX + 1, coordY),
                    IsRoomWithCoordsValidAsync(coordX, coordY + 1),
                    IsRoomWithCoordsValidAsync(coordX, coordY - 1),
                    _roomValidateService.CanEnterRoomFromDirection(coordX - 1, coordY, RoomDirection.Left),
                    _roomValidateService.CanEnterRoomFromDirection(coordX + 1, coordY, RoomDirection.Right),
                    _roomValidateService.CanEnterRoomFromDirection(coordX, coordY + 1, RoomDirection.Top),
                    _roomValidateService.CanEnterRoomFromDirection(coordX, coordY - 1, RoomDirection.Bottom),
                    currRoom));

            return await roomUpdateAsyncContext.ExecuteStrategy();
        }

        public RoomDto SetHasNeighborsProperties(RoomDto room, RoomNavDto roomNav)
        {
            room.HasLeftNeighbor = roomNav.HasLeftNeighbor;
            room.HasRightNeighbor = roomNav.HasRightNeighbor;
            room.HasUpNeighbor = roomNav.HasUpNeighbor;
            room.HasDownNeighbor = roomNav.HasDownNeighbor;
            room.IsLeftDeadEnd = roomNav.IsLeftDeadEnd;
            room.IsRightDeadEnd = roomNav.IsRightDeadEnd;
            room.IsUpDeadEnd = roomNav.IsUpDeadEnd;
            room.IsDownDeadEnd = roomNav.IsDownDeadEnd;
            return room;
        }

        private async Task<RoomNavDto> InitializeCheckRoomAsync(int coordX, int coordY)
        {
            var room = await _roomRepository.GetFirstAsync(
                new RoomCoordsSpecification(coordX, coordY));

            RoomNavDto roomDto = room;
            roomDto.HasLeftNeighbor = false;
            roomDto.HasRightNeighbor = false;
            roomDto.HasUpNeighbor = false;
            roomDto.HasDownNeighbor = false;

            return roomDto;
        }

        private async Task<bool> IsRoomWithCoordsValidAsync(int coordX, int coordY)
            => await _roomRepository.DoCoordsExistByAsync(
                new RoomCoordsSpecification(coordX, coordY));
    }
}
