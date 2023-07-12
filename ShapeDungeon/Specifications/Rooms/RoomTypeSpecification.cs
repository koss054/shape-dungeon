using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;

namespace ShapeDungeon.Specifications.Rooms
{
    public class RoomTypeSpecification : IRoomSpecification
    {
        private readonly RoomActiveType _roomActiveType;

        public RoomTypeSpecification(RoomActiveType roomActiveType)
        {
            _roomActiveType = roomActiveType;
        }

        public bool IsSatisfiedBy(Room room)
        {
            switch(_roomActiveType)
            {
                case RoomActiveType.Move:
                    return room.IsActiveForMove;
                case RoomActiveType.Scout:
                    return room.IsActiveForScout;
                case RoomActiveType.Edit:
                    return room.IsActiveForEdit;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
