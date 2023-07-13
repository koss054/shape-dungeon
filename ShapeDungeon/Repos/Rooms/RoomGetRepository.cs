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

        public async Task<Room> GetFirstOrDefaultByAsync(IRoomSpecification specification)
        {
            var rooms = await Context.Rooms.ToListAsync();
            var roomToReturn = rooms
                .FirstOrDefault(x => specification.IsSatisfiedBy(x));

            return roomToReturn ?? throw new ArgumentNullException(
                nameof(roomToReturn), "No room matches provided specification.");
        }
    }
}
