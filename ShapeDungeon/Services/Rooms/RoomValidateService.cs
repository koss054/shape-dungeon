using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Specifications.Rooms;

namespace ShapeDungeon.Services.Rooms
{
    public class RoomValidateService : IRoomValidateService
    {
        private readonly IRepositoryGet<Room> _roomGetRepository;
        private readonly IRepositoryCoordsGet<Room> _roomCoordsGetRepository;

        public RoomValidateService(
            IRepositoryGet<Room> roomGetRepository,
            IRepositoryCoordsGet<Room> roomCoordsGetRepository)
        {
            _roomGetRepository = roomGetRepository;
            _roomCoordsGetRepository = roomCoordsGetRepository;
        }

        public async Task<bool> CanEnterRoomFromDirection(int coordX, int coordY, RoomDirection direction)
        {
            var doesRoomExist = await _roomCoordsGetRepository.DoCoordsExistByAsync(
                new RoomCoordsSpecification(coordX, coordY));

            if (!doesRoomExist)
                return false;

            var roomToCheck = await _roomGetRepository.GetFirstAsync(
                new RoomCoordsSpecification(coordX, coordY));

            var canGo = direction switch
            {
                RoomDirection.Left => roomToCheck.CanGoRight,
                RoomDirection.Right => roomToCheck.CanGoLeft,
                RoomDirection.Top => roomToCheck.CanGoDown,
                RoomDirection.Bottom => roomToCheck.CanGoUp,
                _ => throw new ArgumentOutOfRangeException(nameof(direction)),
            };

            return canGo;
        }
    }
}
