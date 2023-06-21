using ShapeDungeon.DTOs.Rooms;

namespace ShapeDungeon.Interfaces.Services.Rooms
{
    public interface IGetRoomService
    {
        Task<RoomDto> GetActiveForMoveAsync();
        Task<RoomDto> GetActiveForScoutAsync();
        Task<RoomDetailsDto> GetActiveForEditAsync();
        Task<Guid> GetActiveForMoveId();
        Task<Guid> GetActiveForScoutId();
    }
}