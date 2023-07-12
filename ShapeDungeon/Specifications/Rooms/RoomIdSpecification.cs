using ShapeDungeon.Entities;

namespace ShapeDungeon.Specifications.Rooms
{
    public class RoomIdSpecification : IRoomSpecification
    {
        private readonly Guid _id;

        public RoomIdSpecification(Guid id)
        {
            _id = id;
        }

        public bool IsSatisfiedBy(Room room)
        {
            return room.Id.Equals(_id);
        }
    }
}
