using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;

namespace ShapeDungeon.Services.Rooms
{
    public class RoomTravelService : IRoomTravelService
    {
        private readonly IDbContext _context;

        public RoomTravelService(IDbContext context)
        {
            _context = context;
        }

        public async Task RoomTravelAsync(RoomDirection direction, RoomTravelAction action)
        {
            var oldRoom = action switch
            {
                RoomTravelAction.Move => await _context.Rooms
                                        .SingleOrDefaultAsync(x => x.IsActiveForMove),
                RoomTravelAction.Scout => await _context.Rooms
                                        .SingleOrDefaultAsync(x => x.IsActiveForScout),
                _ => throw new ArgumentOutOfRangeException(nameof(action)),
            };

            if (oldRoom == null) 
                throw new ArgumentNullException(nameof(oldRoom));

            var coordX = oldRoom!.CoordX;
            var coordY = oldRoom!.CoordY;

            switch (direction)
            {
                case RoomDirection.Left: 
                    if (oldRoom.CanGoLeft)
                        coordX--;  
                    break;
                case RoomDirection.Right: 
                    if (oldRoom.CanGoRight)
                        coordX++;  
                    break;
                case RoomDirection.Top: 
                    if (oldRoom.CanGoUp)
                        coordY++; 
                    break;
                case RoomDirection.Bottom: 
                    if (oldRoom.CanGoDown)
                        coordY--;  
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(direction));
            }

            var newRoom = await _context.Rooms
                .SingleOrDefaultAsync(x => x.CoordX == coordX && x.CoordY == coordY);

            if (newRoom != null)
            {
                switch (action)
                {
                    case RoomTravelAction.Move:
                        oldRoom.IsActiveForMove = false;
                        newRoom.IsActiveForMove = true;
                        break;
                    case RoomTravelAction.Scout:
                        oldRoom.IsActiveForScout = false;
                        newRoom.IsActiveForScout = true;
                        break;
                    default: throw new ArgumentOutOfRangeException(nameof(action));
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task ResetScoutAsync()
        {
            var activeForScoutRoom = await _context.Rooms
                .SingleOrDefaultAsync(x => x.IsActiveForScout);

            var activeForMoveRoom = await _context.Rooms
                .SingleOrDefaultAsync(x => x.IsActiveForMove);

            if (activeForScoutRoom != null && activeForMoveRoom != null)
            {
                activeForScoutRoom.IsActiveForScout = false;
                activeForMoveRoom.IsActiveForScout = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
