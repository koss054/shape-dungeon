#nullable disable
using ShapeDungeon.Interfaces.Strategies;

namespace ShapeDungeon.Strategies.Updates
{
    public class UpdateAsyncContext<TEntity>
        where TEntity : class
    {
        private IUpdateAsyncStrategy<TEntity> _strategy;

        public UpdateAsyncContext()
        {
        }

        public UpdateAsyncContext(IUpdateAsyncStrategy<TEntity> strategy)
        {
            _strategy = strategy;
        }

        public void SetStrategy(IUpdateAsyncStrategy<TEntity> strategy)
            => _strategy = strategy;

        public async Task<TEntity> ExecuteStrategy()
            => await _strategy.UpdateObject();
    }
}
