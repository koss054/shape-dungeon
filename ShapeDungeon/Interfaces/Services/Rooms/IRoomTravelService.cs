using ShapeDungeon.Helpers.Enums;

namespace ShapeDungeon.Interfaces.Services.Rooms
{
    public interface IRoomTravelService
    {
        Task RoomTravelAsync(RoomDirection direction, RoomTravelAction action);
        Task ResetScoutAsync();
    }
}
