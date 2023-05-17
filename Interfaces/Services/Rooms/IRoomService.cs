using ShapeDungeon.DTOs.Room;

namespace ShapeDungeon.Interfaces.Services.Rooms
{
    public interface IRoomService
    {
        Task ApplyActiveForEditAsync(Guid roomId);
    }
}
