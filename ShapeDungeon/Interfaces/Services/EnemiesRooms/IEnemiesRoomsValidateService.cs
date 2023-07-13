namespace ShapeDungeon.Interfaces.Services.EnemiesRooms
{
    public interface IEnemiesRoomsValidateService
    {
        Task<bool> IsRoomEnemyDefeated(Guid roomId);
    }
}
