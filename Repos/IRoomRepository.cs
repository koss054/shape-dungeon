using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Entity;

namespace ShapeDungeon.Repos
{
    public interface IRoomRepository
    {
        #region Get methods
        /// <summary>
        /// </summary>
        /// <param name="id">Guid for the room's id.</param>
        /// <returns>Room with matching id or null.</returns>
        Task<IRoom?> GetById(Guid id);

        /// <summary>
        /// </summary>
        /// <param name="coordX">CoordX of the Room.</param>
        /// <param name="coordY">CoordY of the ROom.</param>
        /// <returns>Room with matching coords or null.</returns>
        Task<IRoom?> GetByCoords(int coordX, int coordY);

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
        #endregion

        void Update(IRoom room);
    }
}
