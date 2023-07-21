#nullable disable
using ShapeDungeon.Interfaces.Strategies;

namespace ShapeDungeon.Strategies.Updates
{
    public class UpdateContext<TEntity>
        where TEntity : class
    {
        private IUpdateStrategy<TEntity> _strategy;

        public UpdateContext()
        {
        }

        public UpdateContext(IUpdateStrategy<TEntity> strategy)
        {
            _strategy = strategy;
        }

        public void SetStrategy(IUpdateStrategy<TEntity> strategy)
            => _strategy = strategy;

        public TEntity ExecuteStrategy()
            => _strategy.UpdateObject();
    }
}
