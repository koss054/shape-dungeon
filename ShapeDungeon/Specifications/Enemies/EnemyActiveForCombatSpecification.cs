using ShapeDungeon.Entities;
using System.Linq.Expressions;

namespace ShapeDungeon.Specifications.Enemies
{
    public class EnemyActiveForCombatSpecification : Specification<Enemy>
    {
        public EnemyActiveForCombatSpecification()
        {
        }

        public override bool IsSatisfiedBy(Enemy enemy)
            => enemy.IsActiveForCombat;

        public override Expression<Func<Enemy, bool>> ToExpression()
            => enemy => enemy.IsActiveForCombat;
    }
}
