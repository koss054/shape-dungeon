﻿using ShapeDungeon.Helpers.Enums;
using ShapeDungeon.Interfaces.Entity;

namespace ShapeDungeon.Entities
{
    public class Player : IPlayer
    {
        public Guid Id { get; set; }

        public bool IsActive { get; set; }

        public bool IsInCombat { get; set; }

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
    }
}
