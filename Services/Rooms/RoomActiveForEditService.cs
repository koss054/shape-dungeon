using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;

namespace ShapeDungeon.Services.Rooms
{
    public class RoomActiveForEditService : IRoomActiveForEditService
    {
        private readonly IDbContext _context;

        public RoomActiveForEditService(IDbContext context)
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

        public async Task MoveActiveForEditAsync(int coordX, int coordY)
        {
            var newRoom = await _context.Rooms
                .SingleOrDefaultAsync(x => x.CoordX == coordX && x.CoordY == coordY);

            if (newRoom != null)
            {
                var isOldActiveForEditRemoved = await RemoveCurrentActiveForEditAsync();
                if (isOldActiveForEditRemoved)
                {
                    newRoom.IsActiveForEdit = true;
                    await _context.SaveChangesAsync();
                }
            }
        }
        
        // No _context.SaveChangesAsync(), as we can't leave the DB without an IsActiveForEdit = true.
        private async Task<bool> RemoveCurrentActiveForEditAsync()
        {
            var oldRoom = await _context.Rooms.FirstOrDefaultAsync(x => x.IsActiveForEdit);
            if (oldRoom == null)
                return false;

            oldRoom.IsActiveForEdit = false;
            return true;
        }
    }
}
