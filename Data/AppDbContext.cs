#nullable disable

using Microsoft.EntityFrameworkCore;

using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;

namespace ShapeDungeon.Data
{
    public class AppDbContext : DbContext, IDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EnemyRoom>()
                .HasKey(x => new { x.EnemyId, x.RoomId });
        }

        public DbSet<Enemy> Enemies { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<EnemyRoom> EnemiesRooms { get; set; }
    }
}
