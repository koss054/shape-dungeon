using ShapeDungeon.DTOs.Player;

namespace ShapeDungeon.Interfaces.Services.Players
{
    public interface IPlayerService
    {
        Task<bool> CreatePlayerAsync(PlayerDto pDto);
        Task<IEnumerable<PlayerDto>> GetAllPlayersAsync();
        Task<PlayerDto?> GetPlayerAsync(string name);

        void Dispose();
    }
}
