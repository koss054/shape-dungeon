using Microsoft.EntityFrameworkCore;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Repositories;

namespace ShapeDungeon.Repos
{
    public class EnemiesRoomsRepositoryOld : RepositoryBase<EnemyRoom>, IEnemiesRoomsRepository
    {
        public EnemiesRoomsRepositoryOld(IDbContext context) : base(context)
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

            if (enemyRoom == null)
                throw new ArgumentNullException();

            var enemy = await this.Context.Enemies
                .FirstOrDefaultAsync(x => x.Id == enemyRoom.EnemyId);

            return enemy ?? throw new ArgumentNullException();
        }

        public async Task<Guid> GetEnemyIdByRoomId(Guid roomId)
        {
            var enemyRoom = await this.Context.EnemiesRooms
                .FirstOrDefaultAsync(x => x.RoomId == roomId);

            if (enemyRoom == null)
                throw new ArgumentNullException();

            return enemyRoom.EnemyId;
        }

        public async Task<EnemyRoom> GetEntityByRoomId(Guid roomId)
        {
            var enemyRoom = await this.Context.EnemiesRooms
                .FirstOrDefaultAsync(x => x.RoomId == roomId);

            return enemyRoom ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Checks if the enemy in the searched room has been defeated.
        /// </summary>
        /// <param name="roomId">Id of the room in which the enemy status is being checked.</param>
        /// <returns>
        /// If enemy room exists, the value of the IsEnemyDefeated property.
        /// If enemy room doesn't exist, true (dude's been evaporated from existance, lol).
        /// </returns>
        
    }
}
