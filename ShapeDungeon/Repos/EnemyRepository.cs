using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Specifications;

namespace ShapeDungeon.Repos
{
    public class EnemyRepository : 
        RepositoryBase<Enemy>,
        IEnemyRepository
    {
        public EnemyRepository(IDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Enemy>> GetAll()
            => throw new NotImplementedException();

        public async Task<Enemy> GetFirstAsync(ISpecification<Enemy> specification)
        {
            var expression = specification.ToExpression();
            var enemyRoomToReturn = await this.Context.Enemies
                .AsQueryable()
                .Where(expression)
                .FirstOrDefaultAsync();

            return enemyRoomToReturn ?? throw new ArgumentNullException(
                nameof(enemyRoomToReturn), "No enemy room matches provided specification.");
        }

        public async Task<bool> IsValidByAsync(ISpecification<Enemy> specification)
        {
            var expression = specification.ToExpression();
            var boolToReturn = await this.Context.Enemies
                .AsQueryable()
                .Where(expression)
                .AnyAsync();

            return boolToReturn;
        }

        public void Update(Enemy enemy)
        {
            this.Context.Enemies.Update(enemy);
        }

        public async Task AddAsync(Enemy enemy)
            => await this.Context.Enemies.AddAsync(enemy);
    }
}
