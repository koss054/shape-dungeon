using ShapeDungeon.DTOs.Room;
using ShapeDungeon.Helpers.Enums;

namespace ShapeDungeon.Interfaces.Services.Rooms
{
    public interface IRoomCreateService
    {
        Task<Guid> CreateAsync(RoomDetailsDto roomDto);
        Task<RoomDetailsDto> InitializeRoomAsync(RoomDirection roomDirection);
    }
}
