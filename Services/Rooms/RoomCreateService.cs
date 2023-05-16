using ShapeDungeon.DTOs.Room;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;

namespace ShapeDungeon.Services.Rooms
{
    public class RoomCreateService : IRoomCreateService
    {
        private readonly IDbContext _context;

        public RoomCreateService(IDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateRoomAsync(RoomCreateDto roomDto)
        {
            Room room = new()
            {
                IsActive = false,
                IsActiveForEdit = true,
                CanGoLeft = roomDto.CanGoLeft,
                CanGoRight = roomDto.CanGoRight,
                CanGoUp = roomDto.CanGoUp,
                CanGoDown = roomDto.CanGoDown,
                IsStartRoom = roomDto.IsStartRoom,
                IsSafeRoom = roomDto.IsSafeRoom,
                IsEnemyRoom = roomDto.IsEnemyRoom,
                IsEndRoom = roomDto.IsEndRoom,
                Enemy = null,
                LeftRoomId = roomDto.LeftRoomId,
                RightRoomId = roomDto.RightRoomId,
                TopRoomId = roomDto.TopRoomId,
                DownRoomId = roomDto.DownRoomId,
            };

            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
            return room.Id;
        }

        public RoomCreateDto InitializeLeftRoom(Guid rightRoomId)
            => new() { CanGoRight = true, RightRoomId = rightRoomId };

        public RoomCreateDto InitializeRightRoom(Guid leftRoomId)
            => new() { CanGoLeft = true, LeftRoomId = leftRoomId };

        public RoomCreateDto InitializeUpRoom(Guid bottomRoomId)
            => new() { CanGoDown = true, DownRoomId = bottomRoomId };

        public RoomCreateDto InitializeBottomRoom(Guid upRoomId)
            => new() { CanGoUp = true, TopRoomId = upRoomId };
    }
}
