using ShapeDungeon.Entities;
using System.Linq.Expressions;

namespace ShapeDungeon.Specifications.EnemiesRooms
{
    /// <summary>
    /// Gets an EnemyRoom entity by ROOM ID.
    /// </summary>
    public class EnemyRoomIdSpecification : Specification<EnemyRoom>
    {
        private readonly Guid _id;

        public EnemyRoomIdSpecification(Guid id)
        {
            _id = id;
        }

        public override bool IsSatisfiedBy(EnemyRoom enemyRoom)
            => enemyRoom.RoomId.Equals(_id);

        public override Expression<Func<EnemyRoom, bool>> ToExpression()
            => enemyRoom => enemyRoom.RoomId.Equals(_id);
    }
}
