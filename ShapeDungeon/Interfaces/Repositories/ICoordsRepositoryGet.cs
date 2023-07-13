using ShapeDungeon.Entities;
using ShapeDungeon.Specifications;

namespace ShapeDungeon.Interfaces.Repositories
{
    public interface ICoordsRepositoryGet<T>
        where T : class
    {
        Task<int> GetCoordXByAsync(ISpecification<Room> specification);
        Task<int> GetCoordYByAsync(ISpecification<Room> specification);
    }
}
