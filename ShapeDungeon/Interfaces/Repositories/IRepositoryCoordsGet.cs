using ShapeDungeon.Entities;
using ShapeDungeon.Specifications;

namespace ShapeDungeon.Interfaces.Repositories
{
    public interface IRepositoryCoordsGet<T>
        where T : class
    {
        Task<int> GetCoordXByAsync(ISpecification<T> specification);
        Task<int> GetCoordYByAsync(ISpecification<T> specification);
        Task<bool> DoCoordsExistByAsync(ISpecification<T> specification);
    }
}
