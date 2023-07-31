using ShapeDungeon.Entities;
using System.Linq.Expressions;

namespace ShapeDungeon.Specifications.Rooms
{
    public class RoomMoveSpecification : Specification<Room>
    {
        public RoomMoveSpecification()
        {
        }

        public override bool IsSatisfiedBy(Room room)
            => room.IsActiveForMove;

        public override Expression<Func<Room, bool>> ToExpression()
            => room => room.IsActiveForMove;
    }
}
