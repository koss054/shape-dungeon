using Microsoft.EntityFrameworkCore;
using ShapeDungeon.DTOs;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;

namespace ShapeDungeon.Repos
{
    public class CombatRepositoryOld : RepositoryBase<Combat>, ICombatRepositoryOld
    {
        public CombatRepositoryOld(IDbContext context) : base(context)
        {
        }

        public async Task<Combat?> GetActiveCombat()
            => await this.Context.Combats
            .Include(x => x.Player)
            .Include(x => x.Enemy)
            .SingleOrDefaultAsync(x => x.IsActive);

        public async Task CreateCombat(Player player, Enemy enemy, Guid roomId)
        {
            var combat = new Combat
            {
                IsActive = true,
                IsPlayerAttacking = player.Agility >= enemy.Agility,
                PlayerId = player.Id,
                Player = player,
                CurrentPlayerHp = player.Vigor * 2,
                TotalPlayerHp = player.Vigor * 2,
                EnemyId = enemy.Id,
                Enemy = enemy,
                CurrentEnemyHp = enemy.CurrentHp,
                TotalEnemyHp = enemy.CurrentHp,
                CombatRoomId = roomId,
            };

            await this.Context.Combats.AddAsync(combat);
        }

        public async Task<bool> IsActiveCombatPresent()
            => await this.Context.Combats.AnyAsync(x => x.IsActive);
    }
}
