namespace ShapeDungeon.Interfaces.Strategies
{
    public interface IUpdateAsyncStrategy<TEntity>
        where TEntity : class
    {
        Task<TEntity> UpdateObject();
    }
}
