using ShapeDungeon.Entities;
using System.Linq.Expressions;

namespace ShapeDungeon.Specifications.Players
{
    public class PlayerIdSpecification : Specification<Player>
    {
        private readonly Guid _id;

        public PlayerIdSpecification(Guid id)
        {
            _id = id;
        }

        public override bool IsSatisfiedBy(Player player)
            => player.Id.Equals(_id);

        public override Expression<Func<Player, bool>> ToExpression()
            => player => player.Id.Equals(_id);
    }
}
