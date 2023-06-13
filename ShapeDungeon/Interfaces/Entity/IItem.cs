namespace ShapeDungeon.Interfaces.Entity
{
    public interface IItem : IGuidEntity
    {
        string Name { get; }
        string Description { get; }
        int RequiredLevel { get; }
        int BonusStrength { get; }
        int BonusVigor { get; }
        int BonusAgility { get; }
    }
}
