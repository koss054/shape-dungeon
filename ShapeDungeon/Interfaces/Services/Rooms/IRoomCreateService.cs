using ShapeDungeon.DTOs.Rooms;
using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;

namespace ShapeDungeon.Interfaces.Services.Rooms
{
    public interface IRoomCreateService
    {
        Task<Room> CreateAsync(RoomDetailsDto roomDto);
        Task<RoomDetailsDto> InitializeRoomAsync(RoomDirection roomDirection);
    }
}
