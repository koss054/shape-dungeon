using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Specifications;

namespace ShapeDungeon.Repos
{
    public class EnemyRepository : RepositoryBase<Enemy>, IEnemyRepository
    {
        public EnemyRepository(IDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Enemy>> GetMultipleByAsync(ISpecification<Enemy> specification)
        {
            var expression = specification.ToExpression();
            var enemiesToReturn = await this.Context.Enemies
                .AsQueryable()
                .Where(expression)
                .ToListAsync();

            return enemiesToReturn;
        }

        public async Task<Enemy> GetFirstAsync(ISpecification<Enemy> specification)
        {
            var expression = specification.ToExpression();
            var enemyToReturn = await this.Context.Enemies
                .AsQueryable()
                .Where(expression)
                .FirstOrDefaultAsync();

            return enemyToReturn ?? throw new ArgumentNullException(
                nameof(enemyToReturn), "No enemy matches provided specification.");
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
