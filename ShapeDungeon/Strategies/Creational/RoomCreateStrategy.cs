using ShapeDungeon.DTOs.Rooms;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Strategies;

namespace ShapeDungeon.Strategies.Creational
{
    public class RoomCreateStrategy : ICreateStrategy<Room, RoomDetailsDto>
    {
        private readonly RoomDetailsDto _roomDto;

        public RoomCreateStrategy(RoomDetailsDto roomDto)
        {
            _roomDto = roomDto;
        }

        public Room CreateObject()
        {
            Room room = new()
            {
                IsActiveForMove = false,
                IsActiveForScout = false,
                IsActiveForEdit = _roomDto.IsStartRoom,
                CanGoLeft = _roomDto.CanGoLeft,
                CanGoRight = _roomDto.CanGoRight,
                CanGoUp = _roomDto.CanGoUp,
                CanGoDown = _roomDto.CanGoDown,
                IsStartRoom = _roomDto.IsStartRoom,
                IsSafeRoom = _roomDto.IsSafeRoom,
                IsEnemyRoom = _roomDto.IsEnemyRoom,
                IsEndRoom = _roomDto.IsEndRoom,
                CoordX = _roomDto.CoordX,
                CoordY = _roomDto.CoordY,
            };

            return room;
        }
    }
}
