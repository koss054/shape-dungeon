using ShapeDungeon.DTOs;
using ShapeDungeon.Entities;

namespace ShapeDungeon.Repos
{
    public interface ICombatRepository
    {
        Task<Combat?> GetActiveCombat();
        Task CreateCombat(Player player, Enemy enemy, Guid roomId);
        Task<bool> IsActiveCombatPresent();
    }
}
