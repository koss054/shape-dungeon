using ShapeDungeon.DTOs.Players;

namespace ShapeDungeon.Interfaces.Services.Players
{
    public interface IPlayerGetService
    {
        Task<IEnumerable<PlayerGridDto>> GetAllPlayersAsync();
        Task<PlayerDto> GetPlayerAsync(string name);
        Task<PlayerDto> GetActivePlayer();
    }
}
