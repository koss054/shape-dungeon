using ShapeDungeon.DTOs.Room;

namespace ShapeDungeon.Interfaces.Services.Rooms
{
    public interface IRoomCreateService
    {
        Task<Guid> CreateRoomAsync(RoomCreateDto roomDto);
    }
}
