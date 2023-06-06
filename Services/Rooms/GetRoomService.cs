using ShapeDungeon.DTOs.Rooms;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Repos;

namespace ShapeDungeon.Services.Rooms
{
    public class GetRoomService : IGetRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public GetRoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        public async Task<RoomDto> GetActiveForMoveAsync()
        {
            var room = await _roomRepository.GetActiveForMove();
            RoomDto roomDto = room;
            return roomDto;
        }

        public async Task<RoomDto> GetActiveForScoutAsync()
        {
            var room = await _roomRepository.GetActiveForScout();
            RoomDto roomDto = room; 
            return roomDto;
        }

        public async Task<RoomDetailsDto> GetActiveForEditAsync()
        {
            var room = await _roomRepository.GetActiveForEdit();
            RoomDetailsDto roomDto = room;
            return roomDto;
        }
    }
}
