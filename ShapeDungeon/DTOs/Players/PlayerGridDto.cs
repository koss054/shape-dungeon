﻿using ShapeDungeon.Entities;
using ShapeDungeon.Helpers.Enums;

namespace ShapeDungeon.DTOs.Players
{
    public class PlayerGridDto
    {
        public Guid Id { get; init; }
        public bool IsActive { get; init; }
        public string Name { get; init; }
            = null!;
        public int Level { get; init; }
        public int Strength { get; init; }
        public int Vigor { get; init; }
        public int Agility { get; init; }
        public Shape Shape { get; init; }

        public static implicit operator PlayerGridDto(Player player)
            => new()
            {
                Id = player.Id,
                IsActive = player.IsActive,
                Name = player.Name,
                Level = player.Level,
                Strength = player.Strength,
                Vigor = player.Vigor,
                Agility = player.Agility,
                Shape = player.Shape,
            };
    }
}
