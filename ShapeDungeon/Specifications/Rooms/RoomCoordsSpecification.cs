using ShapeDungeon.Entities;

namespace ShapeDungeon.Specifications.Rooms
{
    public class RoomCoordsSpecification : IRoomSpecification
    {
        private readonly int _coordX;
        private readonly int _coordY;

        public RoomCoordsSpecification(
            int coordX, 
            int coordY)
        {
            _coordX = coordX;
            _coordY = coordY;
        }

        public bool IsSatisfiedBy(Room room)
        {
            return room.CoordX == _coordX 
                && room.CoordY == _coordY;
        }
    }
}
