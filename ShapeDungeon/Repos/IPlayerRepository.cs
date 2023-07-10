using ShapeDungeon.Entities;

namespace ShapeDungeon.Repos
{
    public interface IPlayerRepository
    {
        /// <summary>
        /// Players need scout energy in order to scout adjacent rooms.
        /// Only one can be active. There can be no active players.
        /// </summary>
        /// <returns>The scout energy of the currently IsActive == true player. If no active player, null.</returns>
        Task<int?> GetActiveScoutEnergy();

        /// <summary>
        /// Only one player can be active.
        /// There can be no active players.
        /// </summary>
        /// <returns>The active player or null.</returns>
        Task<Player?> GetActive();

        /// <summary>
        /// All players have names.
        /// </summary>
        /// <returns>Player if one exists with given name, otherwise null.</returns>
        Task<Player?> GetByName(string name);

        /// <summary>
        /// </summary>
        /// <param name="id">Id of the player that we want to return.</param>
        /// <returns>Player with matching id or null.</returns>
        Task<Player?> GetById(Guid id);

        /// <summary>
        /// Many players can be present and selected.
        /// </summary>
        /// <returns>List of all created players.</returns>
        Task<IEnumerable<Player>> GetAll();

        /// <summary>
        /// Used to check if a player name is present in the database.
        /// </summary>
        /// <param name="name">Name that is being checked.</param>
        /// <returns>True if name is in the database, otherwise false.</returns>
        Task<bool> DoesNameExist(string name);

        Task AddAsync(Player player);
    }
}
