using ShapeDungeon.Entities;

namespace ShapeDungeon.Interfaces.Repositories
{
    public interface IPlayerRepository :
        IRepositoryGet<Player>,
        IRepositoryUpdate<Player>,
        IRepositoryValidate<Player>
    {
    }
}
