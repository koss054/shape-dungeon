using ShapeDungeon.DTOs;

namespace ShapeDungeon.Interfaces.Services
{
    public interface ICombatService
    {
        Task InitializeCombat();
        Task<CombatDto> GetActiveCombat();
        Task<bool> HasPlayerWon();
        Task<bool> IsPlayerAttackingInActiveCombat();
        Task ToggleIsPlayerAttackingInActiveCombat();

        /// <summary>
        /// Updates the health of the character that has been attacked.
        /// </summary>
        /// <param name="hpToReduce">The amount of hp the attacked character has lost.</param>
        /// <param name="characterType"> 
        /// CombatCharacterType.Player == 0. CombatCharacterType.Enemy == 1.
        /// Any other int value will throw an exception.
        /// </param>
        /// <returns>The updated health value of the affected character type.</returns>
        /// <exception cref="ArgumentNullException">No active combat could be found.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Incorrect character type has been passed.</exception>
        Task<int> UpdateHealthAfterAttack(int hpToReduce, int characterType);
    }
}
