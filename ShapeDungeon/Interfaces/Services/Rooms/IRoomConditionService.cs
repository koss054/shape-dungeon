namespace ShapeDungeon.Interfaces.Services.Rooms
{
    public interface IRoomConditionService
    {
        /// <summary>
        /// The app needs to know if it has to show the combat screen.
        /// If the player goes to an enemy room and the enemy hasn't been defeated, combat starts.
        /// </summary>
        /// <returns>True, if enemy room has an active enemy. Otherwise, false.</returns>
        Task<bool> IsCurrentRoomActiveEnemyRoom();
    }
}
