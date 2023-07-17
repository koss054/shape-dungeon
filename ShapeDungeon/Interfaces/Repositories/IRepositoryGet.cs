using ShapeDungeon.Specifications;

namespace ShapeDungeon.Interfaces.Repositories
{
    public interface IRepositoryGet<T>
        where T : class
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> GetFirstAsync(ISpecification<T> specification);
    }
}
