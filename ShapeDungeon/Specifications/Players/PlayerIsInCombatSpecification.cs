using ShapeDungeon.Entities;
using System.Linq.Expressions;

namespace ShapeDungeon.Specifications.Players
{
    public class PlayerIsInCombatSpecification : Specification<Player>
    {
        public PlayerIsInCombatSpecification()
        {
        }

        public override bool IsSatisfiedBy(Player player)
            => player.IsInCombat;

        public override Expression<Func<Player, bool>> ToExpression()
            => player => player.IsInCombat;
    }
}
