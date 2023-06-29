using ShapeDungeon.DTOs.Players;

namespace ShapeDungeon.Interfaces.Services.Players
{
    public interface IPlayerCreateService
    {
        Task<bool> CreatePlayerAsync(PlayerDto pDto);
    }
}
