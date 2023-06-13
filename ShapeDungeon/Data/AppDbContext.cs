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

            // TODO: Move to a separate class to keep this method small.
            builder.Entity<Room>()
                .HasData(new Room
                {
                    Id = new Guid("DD54F8EE-349F-4DFD-1A70-08DB56FB8A4B"),
                    IsActiveForMove = true,
                    IsActiveForScout = true,
                    IsActiveForEdit = true,
                    CanGoLeft = true,
                    CanGoRight = true,
                    CanGoUp = true,
                    CanGoDown = true,
                    IsStartRoom = true,
                    IsEnemyRoom = false,
                    IsSafeRoom = false,
                    IsEndRoom = false,
                    Enemy = null,
                    CoordX = 0,
                    CoordY = 0,
                });

            builder.Entity<Player>()
                .HasData(new Player
                {
                    Id = new Guid("3DE35703-1FEF-4070-D75D-08DB4BEAC0A7"),
                    IsActive = true,
                    Name = "Squary Lvl.8",
                    Strength = 2,
                    Vigor = 5,
                    Agility = 1,
                    CurrentExp = 0,
                    ExpToNextLevel = 100,
                    CurrentSkillpoints = 0,
                    CurrentScoutEnergy = 1,
                    Shape = 0,
                });
        }

        public DbSet<Enemy> Enemies { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<EnemyRoom> EnemiesRooms { get; set; }
    }
}
