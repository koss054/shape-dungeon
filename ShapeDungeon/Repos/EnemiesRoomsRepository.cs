using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;

namespace ShapeDungeon.Repos
{
    public class EnemiesRoomsRepository : RepositoryBase<EnemyRoom>, IEnemiesRoomsRepository
    {
        public EnemiesRoomsRepository(IDbContext context) : base(context)
        {
        }

        public async Task AddAsync(Room room, Enemy enemy)
            => await this.Context.EnemiesRooms.AddAsync(new EnemyRoom()
            {
                Room = room,
                RoomId = room.Id,
                Enemy = enemy,
                EnemyId = enemy.Id,
            });

        public async Task<Enemy> GetEnemyByRoomId(Guid roomId)
        {
            var enemyRoom = await this.Context.EnemiesRooms
                .FirstOrDefaultAsync(x => x.RoomId == roomId);

            // Gotta do some error exception work after I finish implementing enemy stuff.
            if (enemyRoom == null)
                return new Enemy();

            var enemy = await this.Context.Enemies
                .FirstOrDefaultAsync(x => x.Id == enemyRoom.EnemyId);

            return enemy ?? new Enemy();
        }
    }
}
