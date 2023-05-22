using ShapeDungeon.DTOs.Room;

namespace ShapeDungeon.Interfaces.Services.Rooms
{
    public interface ICheckRoomNeighborsService
    {
        Task<RoomNavDto?> SetDtoNeighborsAsync(int coordX, int coordY);
        RoomDto SetHasNeighborsProperties(RoomDto room, RoomNavDto roomNavDto);
    }
}
