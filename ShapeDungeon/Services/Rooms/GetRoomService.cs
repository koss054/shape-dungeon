using ShapeDungeon.DTOs.Rooms;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;
using ShapeDungeon.Specifications.Rooms;

namespace ShapeDungeon.Services.Rooms
{
    public class GetRoomService : IGetRoomService
    {
        private readonly IRepositoryGet<Room> _roomGetRepository;

        public GetRoomService(IRepositoryGet<Room> roomGetRepository)
        {
            _roomGetRepository = roomGetRepository;
        }

        public async Task<RoomDto> GetActiveForMoveAsync()
        {
            var room = await _roomGetRepository.GetFirstAsync(
                new RoomMoveSpecification());

            RoomDto roomDto = room;
            return roomDto;
        }

        public async Task<RoomDto> GetActiveForScoutAsync()
        {
            var room = await _roomGetRepository.GetFirstAsync(
                new RoomScoutSpecification());

            RoomDto roomDto = room; 
            return roomDto;
        }

        public async Task<RoomDetailsDto> GetActiveForEditAsync()
        {
            var room = await _roomGetRepository.GetFirstAsync(
                new RoomEditSpecification());

            RoomDetailsDto roomDto = room;
            return roomDto;
        }

        public async Task<Guid> GetActiveForMoveId()
        {
            var room = await _roomGetRepository.GetFirstAsync(
                new RoomMoveSpecification());

            return room.Id;
        }

        public async Task<Guid> GetActiveForScoutId()
        {
            var room = await _roomGetRepository.GetFirstAsync(
                new RoomScoutSpecification());

            return room.Id;
        }

        public async Task<Guid> GetActiveForEditId()
        {
            var room = await _roomGetRepository.GetFirstAsync(
                new RoomEditSpecification());

            return room.Id;
        }
    }
}
