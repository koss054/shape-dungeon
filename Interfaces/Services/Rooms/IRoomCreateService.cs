using ShapeDungeon.DTOs.Room;
using ShapeDungeon.Helpers.Enums;

namespace ShapeDungeon.Interfaces.Services.Rooms
{
    public interface IRoomCreateService
    {
        Task<Guid> CreateAsync(RoomCreateDto roomDto);
        Task<RoomCreateDto> InitializeRoomAsync(RoomDirection roomDirection);
    }
}
