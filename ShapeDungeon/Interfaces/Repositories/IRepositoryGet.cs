using ShapeDungeon.Entities;
using ShapeDungeon.Specifications.Rooms;

namespace ShapeDungeon.Interfaces.Repositories
{
    public interface IRepositoryGet<T>
        where T : class
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> GetFirstOrDefaultByAsync(IRoomSpecification specification);
    }
}
