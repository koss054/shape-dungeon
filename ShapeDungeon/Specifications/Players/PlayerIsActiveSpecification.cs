using ShapeDungeon.Entities;
using System.Linq.Expressions;

namespace ShapeDungeon.Specifications.Players
{
    public class PlayerIsActiveSpecification : Specification<Player>
    {
        public PlayerIsActiveSpecification()
        {
        }

        public override bool IsSatisfiedBy(Player player)
            => player.IsActive;

        public override Expression<Func<Player, bool>> ToExpression()
            => player => player.IsActive;
    }
}
