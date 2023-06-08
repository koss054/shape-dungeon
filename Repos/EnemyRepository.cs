using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;

namespace ShapeDungeon.Repos
{
    public class EnemyRepository : RepositoryBase<Enemy>, IEnemyRepository
    {
        public EnemyRepository(IDbContext context) : base(context)
        {
        }

        public async Task AddAsync(Enemy enemy)
            => await this.Context.Enemies.AddAsync(enemy);
    }
}
