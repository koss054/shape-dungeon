using Microsoft.EntityFrameworkCore;
using ShapeDungeon.DTOs.Room;
using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;
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

        public async Task<Guid> CreateAsync(RoomDetailsDto roomDto)
        {
            Room room = new()
            {
                IsActiveForMove = false,
                IsActiveForScout = false,
                IsActiveForEdit = roomDto.IsStartRoom,
                CanGoLeft = roomDto.CanGoLeft,
                CanGoRight = roomDto.CanGoRight,
                CanGoUp = roomDto.CanGoUp,
                CanGoDown = roomDto.CanGoDown,
                IsStartRoom = roomDto.IsStartRoom,
                IsSafeRoom = roomDto.IsSafeRoom,
                IsEnemyRoom = roomDto.IsEnemyRoom,
                IsEndRoom = roomDto.IsEndRoom,
                Enemy = null,
                CoordX = roomDto.CoordX,
                CoordY = roomDto.CoordY,
            };

            if (room.IsStartRoom)
            {
                room.CoordX = 0;
                room.CoordY = 0;
            }
                
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
            return room.Id;
        }

        public async Task<RoomDetailsDto> InitializeRoomAsync(RoomDirection roomDirection)
        {
            var roomDto = new RoomDetailsDto();
            int coordX = await GetActiveForEditCoordX();
            int coordY = await GetActiveForEditCoordY();

            switch (roomDirection)
            {
                case RoomDirection.Left: coordX--; roomDto.CanGoRight = true; break;
                case RoomDirection.Right: coordX++; roomDto.CanGoLeft = true;  break;
                case RoomDirection.Top: coordY++; roomDto.CanGoDown = true; break;
                case RoomDirection.Bottom: coordY--; roomDto.CanGoUp = true; break;
                default: throw new ArgumentOutOfRangeException(nameof(roomDirection));
            }

            roomDto.CoordX = coordX;
            roomDto.CoordY = coordY;
            return roomDto;
        }

        private async Task<int> GetActiveForEditCoordX()
            => await _context.Rooms
                .Where(x => x.IsActiveForEdit)
                .Select(x => x.CoordX)
                .SingleOrDefaultAsync();

        private async Task<int> GetActiveForEditCoordY()
            => await _context.Rooms
                .Where(x => x.IsActiveForEdit)
                .Select(x => x.CoordY)
                .SingleOrDefaultAsync();
    }
}
