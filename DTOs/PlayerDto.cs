using ShapeDungeon.Entities.Enums;

namespace ShapeDungeon.DTOs
{
    public class PlayerDto
    {
        public bool IsActive { get; set; }
        public string Name { get; set; }
            = null!;

        public int Strength { get; set; }
        public int Vigor { get; set; }
        public int Agility { get; set; }
        public int Level { get; set; }
        public int CurrentExp { get; set; }
        public int ExpToNextLevel { get; set; }
        public int CurrentSkillpoints { get; set; }
        public int CurrentScoutEnergy { get; set; }

        public PlayerShape Shape { get; set; }
    }
}
