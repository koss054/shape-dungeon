using ShapeDungeon.Interfaces.Entity;

namespace ShapeDungeon.Entities
{
    public class Item : IItem
    {
        public Guid Id { get; init; }

        public string Name { get; init; }
            = null!;
        public string Description { get; init; }
            = null!;

        public int RequiredLevel { get; init; }

        public int BonusStrength { get; init; }

        public int BonusVigor { get; init; }

        public int BonusAgility { get; init; }

    }
}
