using ShapeDungeon.DTOs.Rooms;
using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;
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
            var room = _roomGetRepository.GetBy(
                new RoomTypeSpecification(RoomActiveType.Move),
                await GetAllRooms());

            RoomDto roomDto = room;
            return roomDto;
        }

        public async Task<RoomDto> GetActiveForScoutAsync()
        {
            var room = _roomGetRepository.GetBy(
                new RoomTypeSpecification(RoomActiveType.Scout),
                await GetAllRooms());

            RoomDto roomDto = room; 
            return roomDto;
        }

        public async Task<RoomDetailsDto> GetActiveForEditAsync()
        {
            var room = _roomGetRepository.GetBy(
                new RoomTypeSpecification(RoomActiveType.Edit),
                await GetAllRooms());

            RoomDetailsDto roomDto = room;
            return roomDto;
        }

        public async Task<Guid> GetActiveForMoveId()
        {
            var room = _roomGetRepository.GetBy(
                new RoomTypeSpecification(RoomActiveType.Move),
                await GetAllRooms());

            return room.Id;
        }

        public async Task<Guid> GetActiveForScoutId()
        {
            var room = _roomGetRepository.GetBy(
                new RoomTypeSpecification(RoomActiveType.Scout),
                await GetAllRooms());

            return room.Id;
        }

        public async Task<Guid> GetActiveForEditId()
        {
            var room = _roomGetRepository.GetBy(
                new RoomTypeSpecification(RoomActiveType.Edit),
                await GetAllRooms());

            return room.Id;
        }

        private async Task<IEnumerable<Room>> GetAllRooms()
            => await _roomGetRepository.GetAll();
    }
}
