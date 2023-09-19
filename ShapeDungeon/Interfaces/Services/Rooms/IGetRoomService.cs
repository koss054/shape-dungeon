using ShapeDungeon.DTOs.Rooms;
using ShapeDungeon.Entities;

namespace ShapeDungeon.Interfaces.Services.Rooms
{
    public interface IGetRoomService
    {
        Task<Room> GetActiveForMoveAsync();
        Task<RoomDto> GetActiveForMoveDtoAsync();
        Task<RoomDto> GetActiveForScoutAsync();
        Task<RoomDetailsDto> GetActiveForEditAsync();
        Task<Guid> GetActiveForMoveId();
        Task<Guid> GetActiveForScoutId();
        Task<Guid> GetActiveForEditId();
    }
}