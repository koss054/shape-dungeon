namespace ShapeDungeon.Interfaces.Services.Rooms
{
    public interface IRoomActiveForEditService
    {
        Task ApplyActiveForEditAsync(Guid roomId);
        Task MoveActiveForEditAsync(int coordX, int coordY);
    }
}
