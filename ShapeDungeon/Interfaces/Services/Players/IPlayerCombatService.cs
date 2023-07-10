namespace ShapeDungeon.Interfaces.Services.Players
{
    public interface IPlayerCombatService
    {
        Task ExitCombat();
        Task GainExp(int gainedExp);
    }
}
