#nullable disable
using ShapeDungeon.Interfaces.Strategies;

namespace ShapeDungeon.Strategies.Creational
{   
    public class CreateContext<TEntity, YDto>
        where TEntity : class
        where YDto : class
    {
        private ICreateStrategy<TEntity, YDto> _strategy;

        public CreateContext()
        {
        }

        public CreateContext(ICreateStrategy<TEntity, YDto> strategy)
        {
            _strategy = strategy;
        }

        public void SetStrategy(ICreateStrategy<TEntity, YDto> strategy)
            => _strategy = strategy;

        public TEntity ExecuteStrategy()
            => _strategy.CreateObject();
    }
}
