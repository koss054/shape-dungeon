﻿using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;
using ShapeDungeon.Specifications;

namespace ShapeDungeon.Repos
{
    public class EnemyRoomRepository : 
        RepositoryBase<EnemyRoom>,
        IRepositoryGet<EnemyRoom>
    {
        public EnemyRoomRepository(IDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<EnemyRoom>> GetAll()
            => throw new NotImplementedException();

        public async Task<EnemyRoom> GetFirstOrDefaultByAsync(ISpecification<EnemyRoom> specification)
        {
            var expression = specification.ToExpression();
            var enemyRoomToReturn = await this.Context.EnemiesRooms
                .AsQueryable()
                .Where(expression)
                .FirstOrDefaultAsync();

            return enemyRoomToReturn ?? throw new ArgumentNullException(
                nameof(enemyRoomToReturn), "No enemy room matches provided specification.");
        }
    }
}