using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Specifications;

namespace ShapeDungeon.Repos
{
    public class EnemyRoomRepository : 
        RepositoryBase<EnemyRoom>,
        IRepositoryGet<EnemyRoom>,
        IRepositoryValidate<EnemyRoom>
    {
        public EnemyRoomRepository(IDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<EnemyRoom>> GetMultipleByAsync(ISpecification<EnemyRoom> specification)
            => throw new NotImplementedException();

        public async Task<EnemyRoom> GetFirstAsync(ISpecification<EnemyRoom> specification)
        {
            var expression = specification.ToExpression();
            var enemyRoomToReturn = await this.Context.EnemiesRooms
                .Include(x => x.Enemy)
                .Include(x => x.Room)
                .AsQueryable()
                .Where(expression)
                .FirstOrDefaultAsync();

            return enemyRoomToReturn ?? throw new ArgumentNullException(
                nameof(enemyRoomToReturn), "No enemy room matches provided specification.");
        }

        public async Task<bool> IsValidByAsync(ISpecification<EnemyRoom> specification)
        {
            var expression = specification.ToExpression();
            var boolToReturn = await this.Context.EnemiesRooms
                .AsQueryable()
                .Where(expression)
                .AnyAsync();

            return boolToReturn;
        }
    }
}
