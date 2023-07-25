using ShapeDungeon.Interfaces.Repositories;

namespace ShapeDungeon.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContext _context;

        public UnitOfWork(IDbContext context)
        {
            _context = context;
        }

        public async Task Commit(Action action)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                action();
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
