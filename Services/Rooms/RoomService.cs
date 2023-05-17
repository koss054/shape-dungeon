using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;

namespace ShapeDungeon.Services.Rooms
{
    public class RoomService : IRoomService
    {
        private readonly IDbContext _context;

        public RoomService(IDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ChangeActiveForEditRoomAsync(Guid oldRoomId, Guid newRoomId)
        {
            var oldRoom = await _context.Rooms.FindAsync(oldRoomId);
            var newRoom = await _context.Rooms.FindAsync(newRoomId);

            if (oldRoom != null && newRoom != null)
            {
                oldRoom.IsActiveForEdit = false;
                newRoom.IsActiveForEdit = true;

                await _context.SaveChangesAsync();
                return true;
            }

            return false;
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
    }
}
