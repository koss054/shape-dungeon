using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ShapeDungeon.Entities;

namespace ShapeDungeon.Interfaces.Repositories
{
    public interface IDbContext
    {
        public DbSet<Enemy> Enemies { get; }
        public DbSet<Item> Items { get; }
        public DbSet<Player> Players { get; }
        public DbSet<Room> Rooms { get; }
        public DbSet<EnemyRoom> EnemiesRooms { get; }
        public DbSet<Combat> Combats { get; }


        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        void Dispose();
        DatabaseFacade Database { get;  }
    }
}
