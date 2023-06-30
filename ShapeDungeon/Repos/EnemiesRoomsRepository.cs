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
                IsEnemyDefeated = false,
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

        public async Task<Guid> GetEnemyIdByRoomId(Guid roomId)
        {
            var enemyRoom = await this.Context.EnemiesRooms
                .FirstOrDefaultAsync(x => x.RoomId == roomId);

            if (enemyRoom == null)
                throw new ArgumentNullException();

            return enemyRoom.EnemyId;
        }

        /// <summary>
        /// Checks if the enemy in the searched room has been defeated.
        /// </summary>
        /// <param name="roomId">Id of the room in which the enemy status is being checked.</param>
        /// <returns>
        /// If enemy room exists, the value of the IsEnemyDefeated property.
        /// If enemy room doesn't exist, true (dude's been evaporated from existance, lol).
        /// </returns>
        public async Task<bool> IsRoomEnemyDefeated(Guid roomId)
        {
            var enemyRoom = await this.Context.EnemiesRooms
                .FirstOrDefaultAsync(x => x.RoomId == roomId);

            return enemyRoom != null ? enemyRoom.IsEnemyDefeated : true;
        }
    }
}
