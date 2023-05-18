using Microsoft.EntityFrameworkCore;
using ShapeDungeon.DTOs.Room;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;

namespace ShapeDungeon.Services.Rooms
{
    public class CheckRoomNeighborsService : ICheckRoomNeighborsService
    {
        private readonly IDbContext _context;

        public CheckRoomNeighborsService(IDbContext context)
        {
            _context = context;
        }

        public async Task<RoomNavDto?> SetDtoNeighborsAsync(int coordX, int coordY)
        {
            var currRoom = await InitializeCheckRoomAsync(coordX, coordY);

            if (currRoom != null)
            {
                if (currRoom.CanGoLeft)
                    currRoom.HasLeftNeighbor = await IsRoomWithCoordsValidAsync(coordX - 1, coordY);

                if (currRoom.CanGoRight)
                    currRoom.HasRightNeighbor = await IsRoomWithCoordsValidAsync(coordX + 1, coordY);

                if (currRoom.CanGoUp)
                    currRoom.HasUpNeighbor = await IsRoomWithCoordsValidAsync(coordX, coordY - 1);

                if (currRoom.CanGoDown)
                    currRoom.HasDownNeighbor = await IsRoomWithCoordsValidAsync(coordX, coordY + 1);
            }

            return currRoom;
        }

        private async Task<RoomNavDto?> InitializeCheckRoomAsync(int coordX, int coordY)
        {
            var room = await _context.Rooms
                .Where(x => x.CoordX == coordX && x.CoordY == coordY)
                .Select(x => new RoomNavDto()
                {
                    CoordX = x.CoordX,
                    CoordY = x.CoordY,
                    CanGoLeft = x.CanGoLeft,
                    CanGoRight = x.CanGoRight,
                    CanGoUp = x.CanGoUp,
                    CanGoDown = x.CanGoDown,
                    HasLeftNeighbor = false,
                    HasRightNeighbor = false,
                    HasUpNeighbor = false,
                    HasDownNeighbor = false,
                })
                .SingleOrDefaultAsync();

            return room;
        }

        private async Task<bool> IsRoomWithCoordsValidAsync(int coordX, int coordY)
        {
            var room = await _context.Rooms
                .Where(x => x.CoordX == coordX && x.CoordY == coordY)
                .Select(x => new RoomCoordsDto()
                {
                    CoordX = x.CoordX,
                    CoordY = x.CoordY,
                })
                .SingleOrDefaultAsync();

            return room != null;
        }
    }
}
