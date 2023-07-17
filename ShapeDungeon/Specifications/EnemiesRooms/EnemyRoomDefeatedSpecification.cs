using ShapeDungeon.Entities;
using System.Linq.Expressions;

namespace ShapeDungeon.Specifications.EnemiesRooms
{
    public class EnemyRoomDefeatedSpecification : Specification<EnemyRoom>
    {
        public EnemyRoomDefeatedSpecification()
        {
        }

        public override bool IsSatisfiedBy(EnemyRoom enemyRoom)
            => enemyRoom.IsEnemyDefeated;

        public override Expression<Func<EnemyRoom, bool>> ToExpression()
            => enemyRoom => enemyRoom.IsEnemyDefeated;
    }
}
