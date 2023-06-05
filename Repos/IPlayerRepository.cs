using ShapeDungeon.Interfaces.Entity;

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
        Task<IPlayer?> GetActive();

        /// <summary>
        /// All players have names.
        /// </summary>
        /// <returns>Player if one exists with given name, otherwise null.</returns>
        Task<IPlayer?> GetByName(string name);

        /// <summary>
        /// Many players can be present and selected.
        /// </summary>
        /// <returns>List of all created players.</returns>
        Task<IEnumerable<IPlayer>> GetAll();

        /// <summary>
        /// Used to check if a player name is present in the database.
        /// </summary>
        /// <param name="name">Name that is being checked.</param>
        /// <returns>True if name is in the database, otherwise false.</returns>
        Task<bool> DoesNameExist(string name);

        Task AddAsync(IPlayer player);
    }
}
