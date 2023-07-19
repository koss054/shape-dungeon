using ShapeDungeon.DTOs;
using ShapeDungeon.Entities;

namespace ShapeDungeon.Repos
{
    public interface ICombatRepositoryOld
    {
        Task<Combat?> GetActiveCombat();
        Task CreateCombat(Player player, Enemy enemy, Guid roomId);
        Task<bool> IsActiveCombatPresent();
    }
}
