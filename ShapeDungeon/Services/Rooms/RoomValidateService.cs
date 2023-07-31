using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Specifications.Rooms;

namespace ShapeDungeon.Services.Rooms
{
    public class RoomValidateService : IRoomValidateService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomValidateService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<bool> CanEnterRoomFromDirection(int coordX, int coordY, RoomDirection direction)
        {
            var doesRoomExist = await _roomRepository.DoCoordsExistByAsync(
                new RoomCoordsSpecification(coordX, coordY));

            if (!doesRoomExist)
                return false;

            var roomToCheck = await _roomRepository.GetFirstAsync(
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
