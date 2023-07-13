using ShapeDungeon.Entities;
using System.Linq.Expressions;

namespace ShapeDungeon.Specifications.Rooms
{
    public class RoomCoordsSpecification : Specification<Room>
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

        public override bool IsSatisfiedBy(Room room)
            => room.CoordX.Equals(_coordX) 
            && room.CoordY.Equals(_coordY);

        public override Expression<Func<Room, bool>> ToExpression()
            => room => room.CoordX.Equals(_coordX)
                    && room.CoordY.Equals(_coordY);
    }
}
