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
    }
}
