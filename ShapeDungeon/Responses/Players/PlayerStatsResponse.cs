using ShapeDungeon.Entities;

namespace ShapeDungeon.Responses.Players
{
    public class PlayerStatsResponse
    {
        public int Strength { get; init; }
        public int Vigor { get; init; }
        public int Agility { get; init; }

        public static implicit operator PlayerStatsResponse(Player player)
            => new()
            {
                Strength = player.Strength,
                Vigor = player.Vigor,
                Agility = player.Agility,
            };
    }
}
