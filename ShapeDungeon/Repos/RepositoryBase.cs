using ShapeDungeon.Interfaces.Repositories;

namespace ShapeDungeon.Repos
{
    public abstract class RepositoryBase<T>
        where T : class
    {
        internal RepositoryBase(IDbContext context)
        {
            Context = context;
        }

        protected IDbContext Context { get; }
    }
}
