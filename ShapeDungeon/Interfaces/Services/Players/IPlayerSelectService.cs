namespace ShapeDungeon.Interfaces.Services.Players
{
    public interface IPlayerSelectService
    {
        Task UpdateActivePlayer(Guid newActivePlayerId);
    }
}
