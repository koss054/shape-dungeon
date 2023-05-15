using ShapeDungeon.DTOs.Room;

namespace ShapeDungeon.Interfaces.Services
{
    public interface IRoomService
    {
        Task CreateRoomAsync(RoomCreateDto room);
        Task<bool> ChangeActiveRoomAsync(Guid oldRoomId, Guid newRoomId);
        Task<bool> ChangeActiveForEditRoomAsync(Guid oldRoomId, Guid newRoomId);
        Task<RoomDto?> GetActiveRoomAsync();
        Task<RoomCreateDto?> GetActiveForEditRoomAsync();
        Task<RoomNavDto?> GetRoomNavAsync(Guid id);
    }
}
