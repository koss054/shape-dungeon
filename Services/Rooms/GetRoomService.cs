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
            var roomDto = new RoomDto()
            {
                IsActiveForMove = room.IsActiveForMove,
                IsActiveForScout = room.IsActiveForScout,
                CanGoLeft = room.CanGoLeft,
                CanGoRight = room.CanGoRight,
                CanGoUp = room.CanGoUp,
                CanGoDown = room.CanGoDown,
                IsStartRoom = room.IsStartRoom,
                IsSafeRoom = room.IsSafeRoom,
                IsEnemyRoom = room.IsEnemyRoom,
                IsEndRoom = room.IsEndRoom,
                CoordX = room.CoordX,
                CoordY = room.CoordY,
            };

            return roomDto;
        }

        public async Task<RoomDto> GetActiveForScoutAsync()
        {
            var room = await _roomRepository.GetActiveForScout();
            var roomDto = new RoomDto()
            {
                IsActiveForMove = room.IsActiveForMove,
                IsActiveForScout = room.IsActiveForScout,
                CanGoLeft = room.CanGoLeft,
                CanGoRight = room.CanGoRight,
                CanGoUp = room.CanGoUp,
                CanGoDown = room.CanGoDown,
                IsStartRoom = room.IsStartRoom,
                IsSafeRoom = room.IsSafeRoom,
                IsEnemyRoom = room.IsEnemyRoom,
                IsEndRoom = room.IsEndRoom,
                CoordX = room.CoordX,
                CoordY = room.CoordY,
            };

            return roomDto;
        }

        public async Task<RoomDetailsDto> GetActiveForEditAsync()
        {
            var room = await _roomRepository.GetActiveForEdit();
            var roomDto = new RoomDetailsDto()
            {
                IsActiveForEdit = room.IsActiveForEdit,
                CanGoLeft = room.CanGoLeft,
                CanGoRight = room.CanGoRight,
                CanGoUp = room.CanGoUp,
                CanGoDown = room.CanGoDown,
                IsStartRoom = room.IsStartRoom,
                IsSafeRoom = room.IsSafeRoom,
                IsEnemyRoom = room.IsEnemyRoom,
                IsEndRoom = room.IsEndRoom,
                Enemy = null,
                CoordX = room.CoordX,
                CoordY = room.CoordY,
            };

            return roomDto;
        }
    }
}
