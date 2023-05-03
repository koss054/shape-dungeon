using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Entities;

namespace ShapeDungeon.Interfaces.Repositories
{
    public interface IDbContext
    {
        public DbSet<Enemy> Enemies { get; }
        public DbSet<Item> Items { get; }
        public DbSet<Player> Players { get; }
        public DbSet<Room> Rooms { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
