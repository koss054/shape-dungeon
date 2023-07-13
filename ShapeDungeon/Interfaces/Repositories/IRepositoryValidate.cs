using ShapeDungeon.Specifications;

namespace ShapeDungeon.Interfaces.Repositories
{
    public interface IRepositoryValidate<T>
        where T : class
    {
        Task<bool> IsValidByAsync(ISpecification<T> specification);
    }
}
