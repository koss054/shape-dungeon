using ShapeDungeon.Entities;
using System.Linq.Expressions;

namespace ShapeDungeon.Specifications.Enemies
{
    public class EnemyLevelRangeSpecification : Specification<Enemy>
    {
        private readonly int _minLevel;
        private readonly int _maxLevel;

        public EnemyLevelRangeSpecification(int minLevel, int maxLevel)
        {
            _minLevel = minLevel;
            _maxLevel = maxLevel;
        }

        public override bool IsSatisfiedBy(Enemy enemy)
            => enemy.Level >= _minLevel && enemy.Level <= _maxLevel;

        public override Expression<Func<Enemy, bool>> ToExpression()
            => enemy => enemy.Level >= _minLevel && enemy.Level <= _maxLevel;
    }
}
