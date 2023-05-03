using ShapeDungeon.Interfaces.Entity;

namespace ShapeDungeon.Entities
{
    public class Backpack : IBackpack
    {
        public Guid Id { get; init; }

        public int CurrentCapacity { get; set; }

        public int MaxCapacity { get; set; }

        public IEnumerable<IItem> Items { get; set; } 
            = new List<IItem>();

    }
}
