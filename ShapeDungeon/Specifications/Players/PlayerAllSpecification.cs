using ShapeDungeon.Entities;
using System.Linq.Expressions;

namespace ShapeDungeon.Specifications.Players
{
    /// <summary>
    /// Too lazy to make a repo update that gets all entities.
    /// Will eventually update if any other repo needs all entities with no specification.
    /// </summary>
    public class PlayerAllSpecification : Specification<Player>
    {
        public PlayerAllSpecification()
        {
        }

        public override bool IsSatisfiedBy(Player player)
            => player.IsActive || !player.IsActive;

        public override Expression<Func<Player, bool>> ToExpression()
            => player => player.IsActive || !player.IsActive;
    }
}
