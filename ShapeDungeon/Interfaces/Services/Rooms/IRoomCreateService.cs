using ShapeDungeon.DTOs.Rooms;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Entity;

namespace ShapeDungeon.Interfaces.Services.Rooms
{
    public interface IRoomCreateService
    {
        Task<IRoom> CreateAsync(RoomDetailsDto roomDto);
        Task<RoomDetailsDto> InitializeRoomAsync(RoomDirection roomDirection);
    }
}
