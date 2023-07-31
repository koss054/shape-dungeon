using ShapeDungeon.Entities;
using System.Linq.Expressions;

namespace ShapeDungeon.Specifications.Rooms
{
    public class RoomIdSpecification : Specification<Room>
    {
        private readonly Guid _id;

        public RoomIdSpecification(Guid id)
        {
            _id = id;
        }

        public override bool IsSatisfiedBy(Room room)
            => room.Id.Equals(_id);

        public override Expression<Func<Room, bool>> ToExpression()
            => room => room.Id.Equals(_id);
    }
}
