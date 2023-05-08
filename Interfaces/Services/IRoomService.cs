using ShapeDungeon.DTOs;

namespace ShapeDungeon.Interfaces.Services
{
    public interface IRoomService
    {
        Task CreateRoomAsync(RoomDto room);
        Task<bool> ChangeActiveRoomAsync(Guid oldRoomId, Guid newRoomId);
        Task<RoomDto?> GetActiveRoomAsync();
        Task<RoomNavDto?> GetRoomNavAsync(Guid id);
    }
}
