using ShapeDungeon.Helpers.Enums;

namespace ShapeDungeon.Interfaces.Services.Rooms
{
    public interface IRoomValidateService
    {
        Task<bool> CanEnterRoomFromDirection(int coordX, int coordY, RoomDirection direction);
    }
}
