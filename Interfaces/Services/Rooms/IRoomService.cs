using ShapeDungeon.DTOs.Room;

namespace ShapeDungeon.Interfaces.Services.Rooms
{
    public interface IRoomService
    {
        Task<bool> ChangeActiveRoomAsync(Guid oldRoomId, Guid newRoomId);
        Task<bool> ChangeActiveForEditRoomAsync(Guid oldRoomId, Guid newRoomId);
    }
}
