﻿using ShapeDungeon.Entities;

namespace ShapeDungeon.Interfaces.Entity
{
    public interface IRoom : IGuidEntity
    {
        bool IsActive { get; }

        bool IsActiveForEdit { get; }
        bool CanGoLeft { get; }
        bool CanGoRight { get; }
        bool CanGoUp { get; }
        bool CanGoDown { get; }
        bool IsStartRoom { get; }
        bool IsEnemyRoom { get; }
        bool IsSafeRoom { get; }
        bool IsEndRoom { get; }
        Enemy? Enemy { get; }

        int CoordX { get; }
        int CoordY { get; }
    }
}
