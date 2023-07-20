using ShapeDungeon.DTOs.EnemiesRooms;
using ShapeDungeon.Entities;
using ShapeDungeon.Interfaces.Strategies;

namespace ShapeDungeon.Strategies.Creational
{
    public class EnemyRoomCreateStrategy : ICreateStrategy<EnemyRoom, EnemyRoomDto>
    {
        private readonly EnemyRoomDto _enemyRoomDto;

        public EnemyRoomCreateStrategy(EnemyRoomDto enemyRoomDto)
        {
            _enemyRoomDto = enemyRoomDto;
        }

        public EnemyRoom CreateObject()
        {
            var enemyRoom = new EnemyRoom()
            {
                EnemyId = _enemyRoomDto.EnemyId,
                Enemy = _enemyRoomDto.Enemy,
                RoomId = _enemyRoomDto.RoomId,
                Room = _enemyRoomDto.Room,
                IsEnemyDefeated = false,
            };

            return enemyRoom;
        }
    }
}
