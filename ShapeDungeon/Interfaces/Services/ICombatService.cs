using ShapeDungeon.DTOs;

namespace ShapeDungeon.Interfaces.Services
{
    public interface ICombatService
    {
        Task InitializeCombat();
        Task<CombatDto> GetActiveCombat();
    }
}
