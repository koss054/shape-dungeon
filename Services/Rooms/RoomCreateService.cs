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
            };

            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
            return room.Id;
        }

    }
}
