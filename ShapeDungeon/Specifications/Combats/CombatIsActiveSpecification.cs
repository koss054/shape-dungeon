using ShapeDungeon.Entities;
using System.Linq.Expressions;

namespace ShapeDungeon.Specifications.Combats
{
    public class CombatIsActiveSpecification : Specification<Combat>
    {
        public CombatIsActiveSpecification()
        {
        }

        public override bool IsSatisfiedBy(Combat combat)
            => combat.IsActive;

        public override Expression<Func<Combat, bool>> ToExpression()
            => combat => combat.IsActive;
    }
}
