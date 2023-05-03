using ShapeDungeon.Entities;

namespace ShapeDungeon.Interfaces.Entity
{
    public interface IEnemy : ICharacter
    {
        int DroppedExp { get; }
        Guid RoomId { get; }
        Room? Room { get; }
    }
}
