using ShapeDungeon.DTOs.Room;

namespace ShapeDungeon.Interfaces.Services.Rooms
{
    public interface IRoomCreateService
    {
        // Methods used to initialize room creation page.
        // Populate the RoomCreateDto used in the creation page.
        RoomCreateDto InitializeLeftRoom(Guid rightRoomId);
        RoomCreateDto InitializeRightRoom(Guid leftRoomId);
        RoomCreateDto InitializeUpRoom(Guid bottomRoomId);
        RoomCreateDto InitializeBottomRoom(Guid upRoomId);

        // Method used when user is on room creation page.
        // Happens when a blue "Create" button is pressed.
        Task<Guid> CreateRoomAsync(RoomCreateDto roomDto);
    }
}
