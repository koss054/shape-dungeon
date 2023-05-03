namespace ShapeDungeon.Interfaces.Entity
{
    public interface IBackpack : IGuidEntity
    {
        int CurrentCapacity { get; }
        int MaxCapacity { get; }
        IEnumerable<IItem> Items { get; }
    }
}
