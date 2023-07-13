namespace ShapeDungeon.Interfaces.Repositories
{
    public interface IRepositoryUpdate<T>
        where T : class
    {
        void Update(T entity);

        Task AddAsync(T entity);
    }
}
