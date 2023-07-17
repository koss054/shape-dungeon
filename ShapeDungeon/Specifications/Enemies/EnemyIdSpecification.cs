using ShapeDungeon.Entities;
using System.Linq.Expressions;

namespace ShapeDungeon.Specifications.Enemies
{
    public class EnemyIdSpecification : Specification<Enemy>
    {
        private readonly Guid _id;

        public EnemyIdSpecification(Guid id)
        {
            _id = id;
        }

        public override bool IsSatisfiedBy(Enemy enemy)
            => enemy.Id.Equals(_id);

        public override Expression<Func<Enemy, bool>> ToExpression()
            => enemy => enemy.Id.Equals(_id);
    }
}
