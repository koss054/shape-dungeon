using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Enemy>> GetRangeAsync(int minLevel, int maxLevel)
            => await this.Context.Enemies
                .Where(x => x.Level >= minLevel && x.Level <= maxLevel)
                .OrderBy(x => x.Level)
                .ToListAsync();

        public async Task<Enemy?> GetById(Guid enemyId)
            => await this.Context.Enemies
                .FirstOrDefaultAsync(x => x.Id == enemyId);
    }
}
