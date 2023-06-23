using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Repositories;

namespace ShapeDungeon.Repos
{
    public class RoomRepository : RepositoryBase<Room>, IRoomRepository
    {
        public RoomRepository(IDbContext context) : base(context)
        {
        }

        #region Get methods
        /// <summary>
        /// </summary>
        /// <param name="id">Guid for the room's id.</param>
        /// <returns>Room with matching id.</returns>
        public async Task<Room?> GetById(Guid id)
            => await this.Context.Rooms.FirstOrDefaultAsync(x => x.Id == id);

        /// <summary>
        /// </summary>
        /// <param name="coordX">CoordX of the Room.</param>
        /// <param name="coordY">CoordY of the ROom.</param>
        /// <returns>Room with matching coords or null.</returns>
        public async Task<Room?> GetByCoords(int coordX, int coordY) 
            => await this.Context.Rooms.SingleOrDefaultAsync(x => x.CoordX == coordX && x.CoordY == coordY);

        /// <summary>
        /// Not possible for the room to be null.
        /// Start room will always exist, and it'll have this property set to true.
        /// SingleAsync used since only one room can have this property set to true at a time.
        /// </summary>
        /// <returns>The room in which the player currently is and can move from.</returns>
        public async Task<Room> GetActiveForMove()
            => await this.Context.Rooms.SingleAsync(x => x.IsActiveForMove);

        /// <summary>
        /// Not possible for the room to be null.
        /// Start room will always exist, and it'll have this property set to true.
        /// SingleAsync used since only one room can have this property set to true at a time.
        /// </summary>
        /// <returns>The room in which the player currently is or is scouting from.</returns>
        public async Task<Room> GetActiveForScout()
            => await this.Context.Rooms.SingleAsync(x => x.IsActiveForScout);

        /// <summary>
        /// Not possible for the room to be null.
        /// Start room will always exist, and it'll have this property set to true.
        /// SingleAsync used since only one room can have this property set to true at a time.
        /// </summary>
        /// <returns>The room that is currently active in edit mode.</returns>
        public async Task<Room> GetActiveForEdit()
            => await this.Context.Rooms.SingleAsync(x => x.IsActiveForEdit);

        /// <summary>
        /// Rooms are placed on a coordinate system done with integers.
        /// Only one room can have the property IsActiveForEdit set to true.
        /// </summary>
        /// <returns>Coord X of the room that has IsActiveForEdit == true.</returns>
        public async Task<int> GetActiveForEditCoordX()
            => await this.Context.Rooms
                .Where(x => x.IsActiveForEdit)
                .Select(x => x.CoordX)
                .SingleOrDefaultAsync();

        /// <summary>
        /// Rooms are placed on a coordinate system done with integers.
        /// Only one room can have the property IsActiveForEdit set to true.
        /// </summary>
        /// <returns>Coord Y of the room that has IsActiveForEdit == true.</returns>
        public async Task<int> GetActiveForEditCoordY()
            => await this.Context.Rooms
                .Where(x => x.IsActiveForEdit)
                .Select(x => x.CoordY)
                .SingleOrDefaultAsync();
        #endregion

        public async Task AddAsync(Room room)
            => await this.Context.Rooms.AddAsync(room);

        public void Update(Room room)
            => this.Context.Rooms.Update(room);

        /// <summary>
        /// Checks if the player can enter a room with provided coords from provided direction.
        /// </summary>
        /// <param name="coordX">The X coordinate of the room that is being checked.</param>
        /// <param name="coordY">The Y coordinate of the room that is being checked.</param>
        /// <param name="direction">The direction from which the player will be leaving their current room.</param>
        /// <returns>True, if there's no dead end. False, if it's a dead end.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If incorrect direction enum is provided exception is thrown.</exception>
        public async Task<bool> CanEnterRoomFromDirection(int coordX, int coordY, RoomDirection direction)
        {
            var room = await this.GetByCoords(coordX, coordY);
            var canGo = false;

            if (room != null)
            {
                canGo = direction switch
                {
                    RoomDirection.Left => room.CanGoRight,
                    RoomDirection.Right => room.CanGoLeft,
                    RoomDirection.Top => room.CanGoDown,
                    RoomDirection.Bottom => room.CanGoUp,
                    _ => throw new ArgumentOutOfRangeException(nameof(direction)),
                };
            }

            return canGo;
        }
    }
}
