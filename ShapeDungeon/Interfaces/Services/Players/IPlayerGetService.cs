using ShapeDungeon.DTOs.Players;
using ShapeDungeon.Responses.Players;

namespace ShapeDungeon.Interfaces.Services.Players
{
    public interface IPlayerGetService
    {
        Task<IEnumerable<PlayerGridDto>> GetAllPlayersAsync();
        Task<PlayerDto> GetPlayerAsync(string name);
        Task<PlayerDto> GetActivePlayer();

        Task<PlayerStatsResponse> GetActivePlayerStats();
    }
}
