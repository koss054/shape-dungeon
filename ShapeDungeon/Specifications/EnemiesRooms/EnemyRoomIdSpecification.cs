using ShapeDungeon.Entities;
using System.Linq.Expressions;

namespace ShapeDungeon.Specifications.EnemiesRooms
{
    /// <summary>
    /// Gets an EnemyRoom entity by ROOM ID.
    /// </summary>
    public class EnemyRoomIdSpecification : Specification<EnemyRoom>
    {
        private readonly Guid _roomId;

        public EnemyRoomIdSpecification(Guid roomId)
        {
            _roomId = roomId;
        }

        public override bool IsSatisfiedBy(EnemyRoom enemyRoom)
            => enemyRoom.RoomId.Equals(_roomId);

        public override Expression<Func<EnemyRoom, bool>> ToExpression()
            => enemyRoom => enemyRoom.RoomId.Equals(_roomId);
    }
}
