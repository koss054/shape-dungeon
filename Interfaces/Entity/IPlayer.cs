using ShapeDungeon.Entities.Enums;

namespace ShapeDungeon.Interfaces.Entity
{
    public interface IPlayer : ICharacter
    {
        int CurrentExp { get; }
        int ExpToNextLevel { get; }
        int CurrentSkillpoints { get; }
        PlayerShape Shape { get; }
    }
}
