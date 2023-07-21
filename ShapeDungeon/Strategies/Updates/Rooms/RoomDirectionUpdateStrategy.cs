using ShapeDungeon.DTOs.Rooms;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Strategies;

namespace ShapeDungeon.Strategies.Updates.Rooms
{
    public class RoomDirectionUpdateStrategy : IUpdateStrategy<RoomDetailsDto>
    {
        private int _coordX;
        private int _coordY;
        private readonly RoomDirection _direction;

        public RoomDirectionUpdateStrategy(
            int coordX,
            int coordY,
            RoomDirection direction)
        {
            _coordX = coordX;
            _coordY = coordY;
            _direction = direction;
        }

        public RoomDetailsDto UpdateObject()
        {
            var roomDto = new RoomDetailsDto();

            switch (_direction)
            {
                case RoomDirection.Left: _coordX--; roomDto.CanGoRight = true; break;
                case RoomDirection.Right: _coordX++; roomDto.CanGoLeft = true; break;
                case RoomDirection.Top: _coordY++; roomDto.CanGoDown = true; break;
                case RoomDirection.Bottom: _coordY--; roomDto.CanGoUp = true; break;
                default: throw new ArgumentOutOfRangeException(nameof(_direction));
            }

            roomDto.CoordX = _coordX;
            roomDto.CoordY = _coordY;
            return roomDto;
        }
    }
}
