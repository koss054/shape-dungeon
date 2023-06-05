using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Interfaces.Entity;
using ShapeDungeon.Interfaces.Repositories;

namespace ShapeDungeon.Repos
{
    public class PlayerRepository : RepositoryBase<IPlayer>, IPlayerRepository
    {
        public PlayerRepository(IDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Players need scout energy in order to scout adjacent rooms.
        /// Only one can be active. There can be no active players.
        /// </summary>
        /// <returns>The scout energy of the currently IsActive == true player. If no active player, null.</returns>
        public async Task<int?> GetActiveScoutEnergy()
            => await this.Context.Players
                .Where(x => x.IsActive)
                .Select(x => x.CurrentScoutEnergy)
                .SingleOrDefaultAsync();

        /// <summary>
        /// Only one player can be active.
        /// There can be no active players.
        /// </summary>
        /// <returns>The active player or null.</returns>
        public async Task<IPlayer?> GetActive()
            => await this.Context.Players
                .SingleOrDefaultAsync(x => x.IsActive);
    }
}
