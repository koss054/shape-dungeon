using ShapeDungeon.Entities;
using ShapeDungeon.Entities.Enums;

namespace ShapeDungeon.DTOs.Players
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

        public Shape Shape { get; set; }

        public static implicit operator PlayerDto(Player player)
            => new()
            {
                IsActive = player.IsActive,
                Name = player.Name,
                Strength = player.Strength,
                Vigor = player.Vigor,
                Agility = player.Agility,
                Level = player.Level,
                CurrentExp = player.CurrentExp,
                ExpToNextLevel = player.ExpToNextLevel,
                CurrentSkillpoints = player.CurrentSkillpoints,
                CurrentScoutEnergy = player.CurrentScoutEnergy,
                Shape = player.Shape,
            };
    }
}
