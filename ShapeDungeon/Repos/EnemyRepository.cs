using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Specifications;

namespace ShapeDungeon.Repos
{
    public class EnemyRepository : 
        RepositoryBase<Enemy>,
        IRepositoryGet<Enemy>
    {
        public EnemyRepository(IDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Enemy>> GetAll()
            => throw new NotImplementedException();

        public async Task<Enemy> GetFirstOrDefaultByAsync(ISpecification<Enemy> specification)
        {
            var expression = specification.ToExpression();
            var enemyRoomToReturn = await this.Context.Enemies
                .AsQueryable()
                .Where(expression)
                .FirstOrDefaultAsync();

            return enemyRoomToReturn ?? throw new ArgumentNullException(
                nameof(enemyRoomToReturn), "No enemy room matches provided specification.");
        }
    }
}
