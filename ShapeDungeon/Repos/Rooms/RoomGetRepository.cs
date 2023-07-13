using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Interfaces.Repositories.Rooms;
using ShapeDungeon.Specifications;

namespace ShapeDungeon.Repos.Rooms
{
    public class RoomGetRepository : RepositoryBase<Room>, 
        IRepositoryGet<Room>, IRoomCoordsRepositoryGet<Room>
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

        public async Task<int> GetCoordXByAsync(ISpecification<Room> specification)
        {
            var expression = specification.ToExpression();
            var coordToReturn = await this.Context.Rooms
                .AsQueryable()
                .Where(expression)
                .Select(x => x.CoordX)
                .FirstOrDefaultAsync();

            return coordToReturn;
        }

        public async Task<int> GetCoordYByAsync(ISpecification<Room> specification)
        {
            var expression = specification.ToExpression();
            var coordToReturn = await this.Context.Rooms
                .AsQueryable()
                .Where(expression)
                .Select(x => x.CoordY)
                .FirstOrDefaultAsync();

            return coordToReturn;
        }
    }
}
