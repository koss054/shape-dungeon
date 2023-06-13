namespace ShapeDungeon.Interfaces.Entity
{
    public interface ICharacter : IGuidEntity
    {
        string Name { get; }
        int Strength { get; }
        int Vigor { get; }
        int Agility { get; }
        int Level { get; }
    }
}
