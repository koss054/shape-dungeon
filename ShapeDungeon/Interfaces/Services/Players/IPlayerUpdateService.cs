using ShapeDungeon.Helpers.Enums;

namespace ShapeDungeon.Interfaces.Services.Players
{
    public interface IPlayerUpdateService
    {
        Task IncreaseStat(CharacterStat statToIncrease);
        Task EnterCombat();
        Task LoseCombat();
        Task LevelUp();
    }
}
