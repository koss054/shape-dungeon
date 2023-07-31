using ShapeDungeon.Entities;
using System.Linq.Expressions;

namespace ShapeDungeon.Specifications.Players
{
    public class PlayerNameSpecification : Specification<Player>
    {
        private readonly string _name;

        public PlayerNameSpecification(string name)
        {
            _name = name;
        }

        public override bool IsSatisfiedBy(Player player)
            => player.Name.Equals(_name);

        public override Expression<Func<Player, bool>> ToExpression()
            => player => player.Name.Equals(_name);
    }
}
