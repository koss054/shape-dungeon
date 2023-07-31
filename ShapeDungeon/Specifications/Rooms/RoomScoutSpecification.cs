using ShapeDungeon.Entities;
using System.Linq.Expressions;

namespace ShapeDungeon.Specifications.Rooms
{
    public class RoomScoutSpecification : Specification<Room>
    {
        public RoomScoutSpecification()
        {
        }

        public override bool IsSatisfiedBy(Room room)
            => room.IsActiveForScout;

        public override Expression<Func<Room, bool>> ToExpression()
            => room => room.IsActiveForScout;
    }
}
