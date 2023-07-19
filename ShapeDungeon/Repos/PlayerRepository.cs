using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Specifications;

namespace ShapeDungeon.Repos
{
    public class PlayerRepository : RepositoryBase<Player>, IPlayerRepository
    {
        public PlayerRepository(IDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Player>> GetMultipleByAsync(ISpecification<Player> specification)
        {
            var expression = specification.ToExpression();
            var playersToReturn = await this.Context.Players
                .AsQueryable()
                .Where(expression)
                .ToListAsync();

            return playersToReturn;
        }

        public async Task<Player> GetFirstAsync(ISpecification<Player> specification)
        {
            var expression = specification.ToExpression();
            var playerToReturn = await this.Context.Players
                .AsQueryable()
                .Where(expression)
                .FirstOrDefaultAsync();

            return playerToReturn ?? throw new ArgumentNullException(
                nameof(playerToReturn), "No enemy matches provided specification.");
        }

        public async Task<bool> IsValidByAsync(ISpecification<Player> specification)
        {
            var expression = specification.ToExpression();
            var boolToReturn = await this.Context.Players
                .AsQueryable()
                .Where(expression)
                .AnyAsync();

            return boolToReturn;
        }

        public void Update(Player player)
        {
            this.Context.Players.Update(player);
        }

        public async Task AddAsync(Player player)
            => await this.Context.Players.AddAsync(player);
    }
}
