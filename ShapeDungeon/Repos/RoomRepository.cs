using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Specifications;

namespace ShapeDungeon.Repos
{
    public class RoomRepository : RepositoryBase<Room>, IRoomRepository
    {
        public RoomRepository(IDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Room>> GetMultipleByAsync(ISpecification<Room> specification)
        {
            var expression = specification.ToExpression();
            var roomsToReturn = await this.Context.Rooms
                .AsQueryable()
                .Where(expression)
                .ToListAsync();

            return roomsToReturn;
        }

        public async Task<Room> GetFirstAsync(ISpecification<Room> specification)
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
                .FirstAsync();

            return coordToReturn;
        }

        public async Task<int> GetCoordYByAsync(ISpecification<Room> specification)
        {
            var expression = specification.ToExpression();
            var coordToReturn = await this.Context.Rooms
                .AsQueryable()
                .Where(expression)
                .Select(x => x.CoordY)
                .FirstAsync();

            return coordToReturn;
        }

        public async Task<bool> DoCoordsExistByAsync(ISpecification<Room> specification)
        {
            var expression = specification.ToExpression();
            var doCoordsExist = await this.Context.Rooms
                .AsQueryable()
                .Where(expression)
                .AnyAsync();

            return doCoordsExist;
        }

        public void Update(Room room)
        {
            this.Context.Rooms.Update(room);
        }

        public async Task AddAsync(Room room)
            => await this.Context.Rooms.AddAsync(room);
    }
}
