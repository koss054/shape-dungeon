using ShapeDungeon.Entities;
using System.Linq.Expressions;

namespace ShapeDungeon.Specifications.EnemiesRooms
{
    /// <summary>
    /// Gets an EnemyRoom entity by ENEMY ID.
    /// </summary>
    public class RoomEnemyIdSpecification : Specification<EnemyRoom>
    {
        private readonly Guid _enemyId;

        public RoomEnemyIdSpecification(Guid enemyId)
        {
            _enemyId = enemyId;
        }

        public override bool IsSatisfiedBy(EnemyRoom enemyRoom)
            => enemyRoom.EnemyId.Equals(_enemyId);

        public override Expression<Func<EnemyRoom, bool>> ToExpression()
            => enemyRoom => enemyRoom.EnemyId.Equals(_enemyId);
    }
}
