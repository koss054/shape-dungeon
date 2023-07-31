using ShapeDungeon.Entities;

namespace ShapeDungeon.Interfaces.Repositories
{
    public interface ICombatRepository :
        IRepositoryGet<Combat>,
        IRepositoryUpdate<Combat>,
        IRepositoryValidate<Combat>
    {
    }
}
