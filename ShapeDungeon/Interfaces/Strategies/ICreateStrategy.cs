namespace ShapeDungeon.Interfaces.Strategies
{
    public interface ICreateStrategy<TEntity, YDto>
        where TEntity : class
        where YDto : class
    {
        TEntity CreateObject();
    }
}
