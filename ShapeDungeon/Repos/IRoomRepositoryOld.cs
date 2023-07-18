using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;

namespace ShapeDungeon.Repos
{
    public interface IRoomRepositoryOld
    {
        #region Get methods
        /// <summary>
        /// </summary>
        /// <param name="id">Guid for the room's id.</param>
        /// <returns>Room with matching id or null.</returns>
        Task<Room?> GetById(Guid id);

        /// <summary>
        /// </summary>
        /// <param name="coordX">CoordX of the Room.</param>
        /// <param name="coordY">CoordY of the ROom.</param>
        /// <returns>Room with matching coords or null.</returns>
        Task<Room?> GetByCoords(int coordX, int coordY);

        /// <summary>
        /// Not possible for the room to be null.
        /// Start room will always exist, and it'll have this property set to true.
        /// SingleAsync used since only one room can have this property set to true at a time.
        /// </summary>
        /// <returns>The room in which the player currently is and can move from.</returns>
        Task<Room> GetActiveForMove();

        /// <summary>
        /// Not possible for the room to be null.
        /// Start room will always exist, and it'll have this property set to true.
        /// SingleAsync used since only one room can have this property set to true at a time.
        /// </summary>
        /// <returns>The room in which the player currently is or is scouting from.</returns>
        Task<Room> GetActiveForScout();

        /// <summary>
        /// Not possible for the room to be null.
        /// Start room will always exist, and it'll have this property set to true.
        /// SingleAsync used since only one room can have this property set to true at a time.
        /// </summary>
        /// <returns>The room that is currently active in edit mode.</returns>
        Task<Room> GetActiveForEdit();

        /// <summary>
        /// Rooms are placed on a coordinate system done with integers.
        /// </summary>
        /// <returns>Coord X of the room that has IsActiveForEdit == true.</returns>
        Task<int> GetActiveForEditCoordX();

        /// <summary>
        /// Rooms are placed on a coordinate system done with integers.
        /// </summary>
        /// <returns>Coord Y of the room that has IsActiveForEdit == true.</returns>
        Task<int> GetActiveForEditCoordY();
        #endregion

        Task AddAsync(Room room);

        void Update(Room room);

        /// <summary>
        /// Checks if the player can enter a room with provided coords from provided direction.
        /// </summary>
        /// <param name="coordX">The X coordinate of the room that is being checked.</param>
        /// <param name="coordY">The Y coordinate of the room that is being checked.</param>
        /// <param name="direction">The direction from which the player will be leaving their current room.</param>
        /// <returns>True, if there's no dead end. False, if it's a dead end.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If incorrect direction enum is provided exception is thrown.</exception>
        Task<bool> CanEnterRoomFromDirection(int coordX, int coordY, RoomDirection direction);
    }
}
