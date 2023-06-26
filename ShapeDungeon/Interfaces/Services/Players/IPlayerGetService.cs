using ShapeDungeon.DTOs.Players;

namespace ShapeDungeon.Interfaces.Services.Players
{
    public interface IPlayerGetService
    {
        Task<IEnumerable<PlayerDto>> GetAllPlayersAsync();
        Task<PlayerDto> GetPlayerAsync(string name);
    }
}
