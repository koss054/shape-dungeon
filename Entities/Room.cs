using System.ComponentModel.DataAnnotations.Schema;
using ShapeDungeon.Interfaces.Entity;

namespace ShapeDungeon.Entities
{
    public class Room : IRoom
    {
        public Guid Id { get; init; }

        public bool IsStartRoom { get; init; }

        public bool IsEnemyRoom { get; init; }

        public bool IsSafeRoom { get; init; }

        public bool IsEndRoom { get; init; }

        public IEnumerable<IEnemy> Enemies { get; init; }
            = new List<IEnemy>();

        [ForeignKey(nameof(PreviousRoom))]
        public Guid PreviousRoomId { get; init; }

        public Room PreviousRoom { get; init; }
            = new Room();

        [ForeignKey(nameof(NextRoom))]
        public Guid NextRoomId { get; init; }

        public Room NextRoom { get; init; }
            = new Room();
    }
}
