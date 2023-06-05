﻿using ShapeDungeon.Entities.Enums;

namespace ShapeDungeon.Interfaces.Entity
{
    public interface IPlayer : ICharacter
    {
        bool IsActive { get; }
        int CurrentExp { get; }
        int ExpToNextLevel { get; }
        int CurrentSkillpoints { get; }
        int CurrentScoutEnergy { get; set; }
        PlayerShape Shape { get; }
    }
}
