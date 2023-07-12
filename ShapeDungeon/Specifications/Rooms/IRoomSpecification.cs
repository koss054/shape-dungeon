using ShapeDungeon.Entities;

namespace ShapeDungeon.Specifications.Rooms
{
    public interface IRoomSpecification
    {
        bool IsSatisfiedBy(Room room);
    }
}
