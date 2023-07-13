using ShapeDungeon.Entities;
using System.Linq.Expressions;

namespace ShapeDungeon.Specifications.Rooms
{
    public class RoomEditSpecification : Specification<Room>
    {
        public RoomEditSpecification()
        {
        }

        public override bool IsSatisfiedBy(Room room)
            => room.IsActiveForEdit;

        public override Expression<Func<Room, bool>> ToExpression()
            => room => room.IsActiveForEdit;
    }
}
