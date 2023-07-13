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

        public RoomValidateService(IRepositoryGet<Room> roomGetRepository)
        {
            _roomGetRepository = roomGetRepository;
        }

        public async Task<bool> CanEnterRoomFromDirection(int coordX, int coordY, RoomDirection direction)
        {
            var roomToCheck = await _roomGetRepository.GetFirstOrDefaultByAsync(
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
