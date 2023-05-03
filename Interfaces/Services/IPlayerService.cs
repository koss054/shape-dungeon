﻿using ShapeDungeon.DTOs;
namespace ShapeDungeon.Interfaces.Services
{
    public interface IPlayerService
    {
        Task<bool> CreatePlayerAsync(PlayerDto pDto);
        Task<IEnumerable<PlayerDto>> GetAllPlayersAsync();
        Task<PlayerDto?> GetPlayerAsync(string name);

        void Dispose();
    }
}
