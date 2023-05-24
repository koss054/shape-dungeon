﻿using ShapeDungeon.Helpers.Enums;

namespace ShapeDungeon.Interfaces.Services.Players
{
    public interface IPlayerScoutService
    {
        Task<int> GetActiveScoutEnergyAsync();
        Task UpdateActiveScoutEnergyAsync(PlayerScoutAction action);
    }
}
