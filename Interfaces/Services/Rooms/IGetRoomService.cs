using ShapeDungeon.DTOs.Room;

namespace ShapeDungeon.Interfaces.Services.Rooms
{
    public interface IGetRoomService
    {
        Task<RoomDto> GetActiveForMoveAsync();
        Task<RoomDto> GetActiveForScoutAsync();
        Task<RoomDetailsDto> GetActiveForEditAsync();
    }
}