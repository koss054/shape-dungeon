namespace ShapeDungeon.Data
{
    public interface IUnitOfWork
    {
        Task Commit(Action action);
    }
}