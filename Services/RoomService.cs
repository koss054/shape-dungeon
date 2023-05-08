using Microsoft.EntityFrameworkCore;
using ShapeDungeon.DTOs;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services;

namespace ShapeDungeon.Services
{
    public class RoomService : IRoomService
    {
        private readonly IDbContext _context;

        public RoomService(IDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ChangeActiveRoomAsync(Guid oldRoomId, Guid newRoomId)
        {
            var oldRoom = await _context.Rooms.FirstOrDefaultAsync(x => x.Id == oldRoomId);
            var newRoom = await _context.Rooms.FirstOrDefaultAsync(x => x.Id == newRoomId);

            if (oldRoom != null && newRoom != null)
            {
                oldRoom.IsActive = false;
                newRoom.IsActive = true;

                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task CreateRoomAsync(RoomDto room)
        {
            var newRoom = new Room()
            {
                IsActive = room.IsActive,
                CanGoLeft = room.CanGoLeft,
                CanGoRight = room.CanGoRight,
                CanGoUp = room.CanGoUp,
                CanGoDown = room.CanGoDown,
                IsStartRoom = room.IsStartRoom,
                IsEnemyRoom = room.IsEnemyRoom,
                IsSafeRoom = room.IsSafeRoom,
                IsEndRoom = room.IsEndRoom,
                Enemy = null,
                LeftRoomId = room.LeftRoomId,
                RightRoomId = room.RightRoomId,
                TopRoomId = room.TopRoomId,
                DownRoomId = room.DownRoomId
            };

            await _context.Rooms.AddAsync(newRoom);
            await _context.SaveChangesAsync();
        }

        public async Task<RoomDto?> GetActiveRoomAsync()
            => await _context.Rooms
                .Where(x => x.IsActive)
                .Select(x => new RoomDto()
                {
                    IsActive = x.IsActive,
                    CanGoLeft = x.CanGoLeft,
                    CanGoRight = x.CanGoRight,
                    CanGoUp = x.CanGoUp,
                    CanGoDown = x.CanGoDown,
                    IsStartRoom = x.IsStartRoom,
                    IsEnemyRoom = x.IsEnemyRoom,
                    IsSafeRoom = x.IsSafeRoom,
                    IsEndRoom = x.IsEndRoom,
                    Enemy = null,
                    LeftRoomId = x.LeftRoomId,
                    RightRoomId = x.RightRoomId,
                    TopRoomId = x.TopRoomId,
                    DownRoomId = x.DownRoomId
                }).FirstOrDefaultAsync();

        public async Task<RoomNavDto?> GetRoomNavAsync(Guid id)
            => await _context.Rooms
                .Where(x => x.Id == id)
                .Select(x => new RoomNavDto()
                {
                    Id = x.Id,
                    LeftRoomId = x.LeftRoomId,
                    RightRoomId = x.RightRoomId,
                    TopRoomId = x.TopRoomId,
                    DownRoomId = x.DownRoomId
                }).FirstOrDefaultAsync();
    }
}
