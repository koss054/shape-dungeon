using System.Linq.Expressions;

namespace ShapeDungeon.Specifications
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T item);
        Expression<Func<T, bool>> ToExpression();
    }
}
