using ShapeDungeon.DTOs.Room;

namespace ShapeDungeon.Interfaces.Services.Rooms
{
    public interface IGetRoomService
    {
        Task<RoomDto?> GetActiveAsync();
        Task<RoomCreateDto?> GetActiveForEditAsync();
    }
}