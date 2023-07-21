namespace ShapeDungeon.Interfaces.Strategies
{
    public interface IUpdateStrategy<TEntity>
        where TEntity : class
    {
        TEntity UpdateObject();
    }
}
