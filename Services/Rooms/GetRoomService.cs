using Microsoft.EntityFrameworkCore;
using ShapeDungeon.DTOs.Room;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Rooms;

namespace ShapeDungeon.Services.Rooms
{
    public class GetRoomService : IGetRoomService
    {
        private readonly IDbContext _context;

        public GetRoomService(IDbContext context)
        {
            _context = context;
        }

        public async Task<RoomDto?> GetActiveForMoveAsync()
            => await _context.Rooms
                .Where(x => x.IsActiveForMove)
                .Select(x => new RoomDto()
                {
                    IsActiveForMove = x.IsActiveForMove,
                    IsActiveForScout = x.IsActiveForScout,
                    CanGoLeft = x.CanGoLeft,
                    CanGoRight = x.CanGoRight,
                    CanGoUp = x.CanGoUp,
                    CanGoDown = x.CanGoDown,
                    IsStartRoom = x.IsStartRoom,
                    IsSafeRoom = x.IsSafeRoom,
                    IsEnemyRoom = x.IsEnemyRoom,
                    IsEndRoom = x.IsEndRoom,
                    CoordX = x.CoordX,
                    CoordY = x.CoordY,
                })
                .SingleOrDefaultAsync();

        public async Task<RoomDto?> GetActiveForScoutAsync()
            => await _context.Rooms
                .Where(x => x.IsActiveForScout)
                .Select(x => new RoomDto()
                {
                    IsActiveForMove = x.IsActiveForMove,
                    IsActiveForScout = x.IsActiveForScout,
                    CanGoLeft = x.CanGoLeft,
                    CanGoRight = x.CanGoRight,
                    CanGoUp = x.CanGoUp,
                    CanGoDown = x.CanGoDown,
                    IsStartRoom = x.IsStartRoom,
                    IsSafeRoom = x.IsSafeRoom,
                    IsEnemyRoom = x.IsEnemyRoom,
                    IsEndRoom = x.IsEndRoom,
                    CoordX = x.CoordX,
                    CoordY = x.CoordY,
                })
                .SingleOrDefaultAsync();

        public async Task<RoomDetailsDto?> GetActiveForEditAsync()
            => await _context.Rooms
                .Where(x => x.IsActiveForEdit)
                .Select(x => new RoomDetailsDto()
                {
                    IsActiveForEdit = x.IsActiveForEdit,
                    CanGoLeft = x.CanGoLeft,
                    CanGoRight = x.CanGoRight,
                    CanGoUp = x.CanGoUp,
                    CanGoDown = x.CanGoDown,
                    IsStartRoom = x.IsStartRoom,
                    IsSafeRoom = x.IsSafeRoom,
                    IsEnemyRoom = x.IsEnemyRoom,
                    IsEndRoom = x.IsEndRoom,
                    Enemy = null,
                    CoordX = x.CoordX,
                    CoordY = x.CoordY,
                })
                .SingleOrDefaultAsync();
    }
}
