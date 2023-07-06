using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;

namespace ShapeDungeon.Repos
{
    public class PlayerRepository : RepositoryBase<Player>, IPlayerRepository
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
        public async Task<Player?> GetActive()
            => await this.Context.Players.SingleOrDefaultAsync(x => x.IsActive);

        /// <summary>
        /// All players have names.
        /// </summary>
        /// <returns>Player if one exists with given name, otherwise null.</returns>
        public async Task<Player?> GetByName(string name)
            => await this.Context.Players.FirstOrDefaultAsync(x => x.Name == name);

        /// <summary>
        /// </summary>
        /// <param name="id">Id of the player that we want to return.</param>
        /// <returns>Player with matching id or null.</returns>
        public async Task<Player?> GetById(Guid id)
            => await this.Context.Players.FirstOrDefaultAsync(x => x.Id == id);

        /// <summary>
        /// Many players can be present and selected.
        /// </summary>
        /// <returns>List of all created players.</returns>
        public async Task<IEnumerable<Player>> GetAll()
            => await this.Context.Players.ToListAsync();

        /// <summary>
        /// Used to check if a player name is present in the database.
        /// </summary>
        /// <param name="name">Name that is being checked.</param>
        /// <returns>True if name is in the database, otherwise false.</returns>
        public async Task<bool> DoesNameExist(string name)
            => await this.Context.Players.AnyAsync(x => x.Name == name);

        public async Task ExitCombat(Guid playerId)
        {
            var player = await this.Context.Players
                .FirstOrDefaultAsync(x => x.Id == playerId);

            if (player != null)
                player.IsInCombat = false;
        }

        public async Task AddAsync(Player player)
            => await this.Context.Players.AddAsync(player);
    }
}
