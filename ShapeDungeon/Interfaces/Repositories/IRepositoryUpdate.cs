namespace ShapeDungeon.Interfaces.Repositories
{
    public interface IRepositoryUpdate<T>
        where T : class
    {
        void Update(T entity);
    }
}
