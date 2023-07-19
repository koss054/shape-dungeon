using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Specifications;

namespace ShapeDungeon.Repos
{
    public class CombatRepository : RepositoryBase<Combat>, ICombatRepository
    {
        public CombatRepository(IDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Combat>> GetMultipleByAsync(ISpecification<Combat> specification)
        {
            var expression = specification.ToExpression();
            var combatsToReturn = await this.Context.Combats
                .Include(x => x.Enemy)
                .Include(x => x.Player)
                .AsQueryable()
                .Where(expression)
                .ToListAsync();

            return combatsToReturn;
        }

        public async Task<Combat> GetFirstAsync(ISpecification<Combat> specification)
        {
            var expression = specification.ToExpression();
            var combatToReturn = await this.Context.Combats
                .Include(x => x.Enemy)
                .Include(x => x.Player)
                .AsQueryable()
                .Where(expression)
                .FirstOrDefaultAsync();

            return combatToReturn ?? throw new ArgumentNullException(
                nameof(combatToReturn), "No enemy matches provided specification.");
        }

        public async Task<bool> IsValidByAsync(ISpecification<Combat> specification)
        {
            var expression = specification.ToExpression();
            var boolToReturn = await this.Context.Combats
                .AsQueryable()
                .Where(expression)
                .AnyAsync();

            return boolToReturn;
        }

        public void Update(Combat combat)
        {
            this.Context.Combats.Update(combat);
        }

        public async Task AddAsync(Combat combat)
            => await this.Context.Combats.AddAsync(combat);
    }
}
