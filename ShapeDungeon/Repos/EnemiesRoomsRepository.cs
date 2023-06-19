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
    }
}
