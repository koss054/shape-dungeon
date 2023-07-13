using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Specifications;

namespace ShapeDungeon.Repos.Rooms
{
    public class RoomGetRepository : RepositoryBase<Room>, IRepositoryGet<Room>
    {
        public RoomGetRepository(IDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Room>> GetAll()
            => await this.Context.Rooms.ToListAsync();

        public async Task<Room> GetFirstOrDefaultByAsync(ISpecification<Room> specification)
        {
            var expression = specification.ToExpression();
            var roomToReturn = await this.Context.Rooms
                .AsQueryable()
                .Where(expression)
                .FirstOrDefaultAsync();

            return roomToReturn ?? throw new ArgumentNullException(
                nameof(roomToReturn), "No room matches provided specification.");
        }
    }
}
