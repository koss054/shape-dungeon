using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Specifications.Rooms;

namespace ShapeDungeon.Repos.Rooms
{
    public class RoomGetRepository : RepositoryBase<Room>, IRepositoryGet<Room>
    {
        public RoomGetRepository(IDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Room>> GetAll()
            => await Context.Rooms.ToListAsync();

        public Room GetBy(
            IRoomSpecification specification,
            IEnumerable<Room> rooms)
        {
            var roomToReturn = new Room();

            foreach (Room room in rooms)
                if (specification.IsSatisfiedBy(room))
                    roomToReturn = room;

            return roomToReturn;
        }
    }
}
