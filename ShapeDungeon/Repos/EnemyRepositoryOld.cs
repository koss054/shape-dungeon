using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;

namespace ShapeDungeon.Repos
{
    public class EnemyRepositoryOld : RepositoryBase<Enemy>, IEnemyRepositoryOld
    {
        public EnemyRepositoryOld(IDbContext context) : base(context)
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

        public async Task<Enemy?> GetActiveForCombat()
            => await this.Context.Enemies
                .SingleOrDefaultAsync(x => x.IsActiveForCombat);

        public async Task SetActiveForCombat(Guid enemyId)
        {
            var enemy = await this.Context.Enemies
                .FirstOrDefaultAsync(x => x.Id == enemyId);

            if (enemy != null)
                enemy.IsActiveForCombat = true;
        }

        public async Task ClearActiveForCombat()
        {
            var activeEnemy = await this.Context.Enemies
                .SingleOrDefaultAsync(x => x.IsActiveForCombat);

            if (activeEnemy != null)
                activeEnemy.IsActiveForCombat = false;
        }
    }
}
