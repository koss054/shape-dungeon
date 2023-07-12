using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;

namespace ShapeDungeon.Repos.Rooms
{
    public class RoomUpdateRepository : RepositoryBase<Room>, IRepositoryUpdate<Room>
    {
        public RoomUpdateRepository(IDbContext context) : base(context)
        {
        }

        public void Update(Room entity)
        {
            this.Context.Rooms.Update(entity);
        }
    }
}
