using ShapeDungeon.DTOs;
using ShapeDungeon.Entities.Enums;

namespace ShapeDungeon.Interfaces.Services
{
    public interface IPlayerService
    {
        Task<bool> CreatePlayerAsync(string name, PlayerShape shape);
        Task<PlayerDto?> GetPlayerAsync(string name);
    }
}
