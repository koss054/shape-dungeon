using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Entity;
using ShapeDungeon.Interfaces.Repositories;

namespace ShapeDungeon.Repos
{
    public class RoomRepository : RepositoryBase<IRoom>, IRoomRepository
    {
        public RoomRepository(IDbContext context) : base(context)
        {
        }

        #region Get methods
        /// <summary>
        /// </summary>
        /// <param name="id">Guid for the room's id.</param>
        /// <returns>Room with matching id.</returns>
        public async Task<IRoom?> GetById(Guid id)
            => await this.Context.Rooms.FirstOrDefaultAsync(x => x.Id == id);

        /// <summary>
        /// </summary>
        /// <param name="coordX">CoordX of the Room.</param>
        /// <param name="coordY">CoordY of the ROom.</param>
        /// <returns>Room with matching coords or null.</returns>
        public async Task<IRoom?> GetByCoords(int coordX, int coordY) 
            => await this.Context.Rooms.SingleOrDefaultAsync(x => x.CoordX == coordX && x.CoordY == coordY);

        /// <summary>
        /// Not possible for the room to be null.
        /// Start room will always exist, and it'll have this property set to true.
        /// SingleAsync used since only one room can have this property set to true at a time.
        /// </summary>
        /// <returns>The room in which the player currently is and can move from.</returns>
        public async Task<IRoom> GetActiveForMove()
            => await this.Context.Rooms.SingleAsync(x => x.IsActiveForMove);

        /// <summary>
        /// Not possible for the room to be null.
        /// Start room will always exist, and it'll have this property set to true.
        /// SingleAsync used since only one room can have this property set to true at a time.
        /// </summary>
        /// <returns>The room in which the player currently is or is scouting from.</returns>
        public async Task<IRoom> GetActiveForScout()
            => await this.Context.Rooms.SingleAsync(x => x.IsActiveForScout);

        /// <summary>
        /// Not possible for the room to be null.
        /// Start room will always exist, and it'll have this property set to true.
        /// SingleAsync used since only one room can have this property set to true at a time.
        /// </summary>
        /// <returns>The room that is currently active in edit mode.</returns>
        public async Task<IRoom> GetActiveForEdit()
            => await this.Context.Rooms.SingleAsync(x => x.IsActiveForEdit);
        #endregion

        public void Update(IRoom room)
        {
            var updatedRoom = (Room)room;
            this.Context.Rooms.Update(updatedRoom);
        }
    }
}
