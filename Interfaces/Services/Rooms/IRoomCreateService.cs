using ShapeDungeon.DTOs.Room;

namespace ShapeDungeon.Interfaces.Services.Rooms
{
    public interface IRoomCreateService
    {
        // Methods used to initialize room creation page.
        // Populate the RoomCreateDto used in the creation page.
        RoomCreateDto InitializeLeftRoom(Guid rightRoomId);
        RoomCreateDto InitializeRightRoom(Guid leftRoomId);
        RoomCreateDto InitializeTopRoom(Guid downRoomId);
        RoomCreateDto InitializeDownRoom(Guid topRoomId);

        Task AddLeftNeighborAsync(Guid oldRoomId, Guid leftRoomId);
        Task AddRightNeighborAsync(Guid oldRoomId, Guid rightRoomId);
        Task AddTopNeighborAsync(Guid oldRoomId, Guid topRoomId);
        Task AddDownNeighborAsync(Guid oldRoomId, Guid downRoomId);

        // Method used when user is on room creation page.
        // Happens when a blue "Create" button is pressed.
        Task<Guid> CreateRoomAsync(RoomCreateDto roomDto);
    }
}
