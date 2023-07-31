using ShapeDungeon.Entities;
using System.Linq.Expressions;

namespace ShapeDungeon.Specifications.EnemiesRooms
{
    public class EnemyRoomDefeatedSpecification : Specification<EnemyRoom>
    {
        private readonly Guid _roomId;

        public EnemyRoomDefeatedSpecification(Guid roomId)
        {
            _roomId = roomId;
        }

        public override bool IsSatisfiedBy(EnemyRoom enemyRoom)
            => enemyRoom.RoomId.Equals(_roomId) && enemyRoom.IsEnemyDefeated;

        public override Expression<Func<EnemyRoom, bool>> ToExpression()
            => enemyRoom => enemyRoom.RoomId.Equals(_roomId) && enemyRoom.IsEnemyDefeated;
    }
}
