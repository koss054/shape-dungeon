﻿using ShapeDungeon.Interfaces.Entity;

namespace ShapeDungeon.Entities
{
    public class Room : IRoom
    {
        public Guid Id { get; init; }

        public bool IsActive { get; set; }

        public bool CanGoLeft { get; set; }

        public bool CanGoRight { get; set; }

        public bool CanGoUp { get; set; }

        public bool CanGoDown { get; set; }

        public bool IsStartRoom { get; init; }

        public bool IsEnemyRoom { get; init; }

        public bool IsSafeRoom { get; init; }

        public bool IsEndRoom { get; init; }

        public IEnumerable<Enemy> Enemies { get; init; }
            = new List<Enemy>();

        public Guid PreviousRoomId { get; init; }

        public Guid NextRoomId { get; init; }
    }
}