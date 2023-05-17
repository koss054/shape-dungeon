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

        public async Task ApplyActiveForEditAsync(Guid roomId)
        {
            var isOldActiveForEditRemoved = await RemoveCurrentActiveForEditAsync();
            if (isOldActiveForEditRemoved)
            {
                var newRoom = await _context.Rooms.FirstOrDefaultAsync(x => x.Id == roomId);

                if (newRoom != null)
                {
                    newRoom.IsActiveForEdit = true;
                    await _context.SaveChangesAsync();
                }
            }
        }

        private async Task<bool> RemoveCurrentActiveForEditAsync()
        {
            var oldRoom = await _context.Rooms.FirstOrDefaultAsync(x => x.IsActiveForEdit);
            if (oldRoom == null)
                return false;

            oldRoom.IsActiveForEdit = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
