using ShapeDungeon.DTOs;

namespace ShapeDungeon.Interfaces.Services
{
    public interface ICombatService
    {
        Task InitializeCombat();
        Task<CombatDto> GetActiveCombat();

        Task Test(int hp);
        Task<int> Test2();
    }
}
