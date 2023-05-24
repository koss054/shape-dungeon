using Microsoft.EntityFrameworkCore;
using ShapeDungeon.DTOs.Player;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Services.Players;

namespace ShapeDungeon.Services.Players
{
    public class PlayerScoutService : IPlayerScoutService
    {
        private readonly IDbContext _context;

        public PlayerScoutService(IDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetActiveScoutEnergyAsync()
        {
            var currActivePlayer = await _context.Players
                .Where(x => x.IsActive)
                .Select(x => new PlayerEnergyDto()
                {
                    CurrentScoutEnergy = x.CurrentScoutEnergy,
                }).SingleOrDefaultAsync();

            return currActivePlayer != null 
                ? currActivePlayer.CurrentScoutEnergy 
                : 0;
        }

        public async Task UpdateActiveScoutEnergyAsync(PlayerScoutAction action)
        {
            var currActivePlayer = await _context.Players
                .SingleOrDefaultAsync(x => x.IsActive);

            if (currActivePlayer == null)
                throw new NullReferenceException(nameof(currActivePlayer));

            var currScoutEnergy = currActivePlayer.CurrentScoutEnergy;

            if (action == PlayerScoutAction.Reduce)
            {
                currScoutEnergy--;
            }
            else if (action == PlayerScoutAction.Refill)
            {
                currScoutEnergy = currActivePlayer.Agility;
            }

            currActivePlayer.CurrentScoutEnergy = currScoutEnergy;
            await _context.SaveChangesAsync();
        }
    }
}
