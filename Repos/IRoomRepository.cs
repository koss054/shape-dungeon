using ShapeDungeon.Interfaces.Entity;

namespace ShapeDungeon.Repos
{
    public interface IRoomRepository
    {
        /// <summary>
        /// Not possible for the room to be null.
        /// Start room will always exist, and it'll have this property set to true.
        /// SingleAsync used since only one room can have this property set to true at a time.
        /// </summary>
        /// <returns>The room in which the player currently is and can move from.</returns>
        Task<IRoom> GetActiveForMove();

        /// <summary>
        /// Not possible for the room to be null.
        /// Start room will always exist, and it'll have this property set to true.
        /// SingleAsync used since only one room can have this property set to true at a time.
        /// </summary>
        /// <returns>The room in which the player currently is or is scouting from.</returns>
        Task<IRoom> GetActiveForScout();

        /// <summary>
        /// Not possible for the room to be null.
        /// Start room will always exist, and it'll have this property set to true.
        /// SingleAsync used since only one room can have this property set to true at a time.
        /// </summary>
        /// <returns>The room that is currently active in edit mode.</returns>
        Task<IRoom> GetActiveForEdit();
    }
}
