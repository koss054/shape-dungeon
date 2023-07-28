using ShapeDungeon.DTOs.Rooms;
using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Specifications;
using ShapeDungeon.Specifications.Rooms;

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

        public async Task<RoomNavDto> SetDtoNeighborsAsync(int coordX, int coordY)
        {
            var currRoom = await InitializeCheckRoomAsync(coordX, coordY);

            if (currRoom.CanGoLeft)
            {
                currRoom.HasLeftNeighbor = await IsRoomWithCoordsValidAsync(
                    new RoomCoordsSpecification(coordX - 1, coordY));

                currRoom.IsLeftDeadEnd = !(await _roomValidateService
                    .CanEnterRoomFromDirection(coordX - 1, coordY, RoomDirection.Left));
            }

            if (currRoom.CanGoRight)
            {
                currRoom.HasRightNeighbor = await IsRoomWithCoordsValidAsync(
                    new RoomCoordsSpecification(coordX + 1, coordY));

                currRoom.IsRightDeadEnd = !(await _roomValidateService
                    .CanEnterRoomFromDirection(coordX + 1, coordY, RoomDirection.Right));
            }

            if (currRoom.CanGoUp)
            {
                currRoom.HasUpNeighbor = await IsRoomWithCoordsValidAsync(
                    new RoomCoordsSpecification(coordX, coordY + 1));

                currRoom.IsUpDeadEnd = !(await _roomValidateService
                    .CanEnterRoomFromDirection(coordX, coordY + 1, RoomDirection.Top));
            }

            if (currRoom.CanGoDown)
            {
                currRoom.HasDownNeighbor = await IsRoomWithCoordsValidAsync(
                    new RoomCoordsSpecification(coordX, coordY - 1));

                currRoom.IsDownDeadEnd = !(await _roomValidateService
                    .CanEnterRoomFromDirection(coordX, coordY - 1, RoomDirection.Bottom));
            }

            return currRoom;
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

        private async Task<bool> IsRoomWithCoordsValidAsync(ISpecification<Room> specification)
            => await _roomRepository.DoCoordsExistByAsync(specification);
    }
}
