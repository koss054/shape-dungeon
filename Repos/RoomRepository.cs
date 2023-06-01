using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Interfaces.Entity;
using ShapeDungeon.Interfaces.Repositories;

namespace ShapeDungeon.Repos
{
    public class RoomRepository : RepositoryBase<IRoom>, IRoomRepository
    {
        public RoomRepository(IDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Not possible for the room to be null.
        /// Start room will always exist, and it'll have this property set to true.
        /// </summary>
        /// <returns>The room in which the player currently is and can move from.</returns>
        public async Task<IRoom> GetActiveForMove()
            => await this.Context.Rooms.SingleAsync(x => x.IsActiveForMove);

        /// <summary>
        /// Not possible for the room to be null.
        /// Start room will always exist, and it'll have this property set to true.
        /// </summary>
        /// <returns>The room in which the player currently is or is scouting from.</returns>
        public async Task<IRoom> GetActiveForScout()
            => await this.Context.Rooms.SingleAsync(x => x.IsActiveForScout);

        /// <summary>
        /// Not possible for the room to be null.
        /// Start room will always exist, and it'll have this property set to true.
        /// </summary>
        /// <returns>The room that is currently active in edit mode.</returns>
        public async Task<IRoom> GetActiveForEdit()
            => await this.Context.Rooms.SingleAsync(x => x.IsActiveForEdit);
    }
}
