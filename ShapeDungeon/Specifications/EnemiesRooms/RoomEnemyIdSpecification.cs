using ShapeDungeon.Entities;
using System.Linq.Expressions;

namespace ShapeDungeon.Specifications.EnemiesRooms
{
    /// <summary>
    /// Gets an EnemyRoom entity by ENEMY ID.
    /// </summary>
    public class RoomEnemyIdSpecification : Specification<EnemyRoom>
    {
        private readonly Guid _id;

        public RoomEnemyIdSpecification(Guid id)
        {
            _id = id;
        }

        public override bool IsSatisfiedBy(EnemyRoom enemyRoom)
            => enemyRoom.EnemyId.Equals(_id);

        public override Expression<Func<EnemyRoom, bool>> ToExpression()
            => enemyRoom => enemyRoom.EnemyId.Equals(_id);
    }
}
