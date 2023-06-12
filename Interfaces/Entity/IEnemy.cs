namespace ShapeDungeon.Interfaces.Entity
{
    public interface IEnemy : ICharacter
    {
        int DroppedExp { get; }
        int CurrentHp { get; }
    }
}
