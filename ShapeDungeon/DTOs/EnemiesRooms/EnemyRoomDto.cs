using ShapeDungeon.Entities;

namespace ShapeDungeon.DTOs.EnemiesRooms
{
    public class EnemyRoomDto
    {
        public EnemyRoomDto()
        {
        }

        public EnemyRoomDto(Enemy enemy, Room room)
        {
            EnemyId = enemy.Id;
            Enemy = enemy;
            RoomId = room.Id;
            Room = room;
        }

        public Guid EnemyId { get; set; }

        public Enemy Enemy { get; set; }
            = null!;

        public Guid RoomId { get; set; }

        public Room Room { get; set; }
            = null!;

        public bool IsEnemyDefeated { get; set; }
    }
}
