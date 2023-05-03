namespace ShapeDungeon.Interfaces.Entity
{
    public interface IRoom : IGuidEntity
    {
        bool IsStartRoom { get; }
        bool IsEnemyRoom { get; }
        bool IsSafeRoom { get; }
        bool IsEndRoom { get; }
        IEnumerable<IEnemy> Enemies { get; }
    }
}
