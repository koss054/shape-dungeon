using System.Linq.Expressions;

namespace ShapeDungeon.Specifications
{
    public class NotSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _specification;

        public NotSpecification(Specification<T> specification)
        {
            _specification = specification 
                ?? throw new ArgumentNullException(nameof(specification));
        }

        public override bool IsSatisfiedBy(T item)
            => !_specification.IsSatisfiedBy(item);

        public override Expression<Func<T, bool>> ToExpression()
        {
            var expression = _specification.ToExpression();
            var paramExpr = expression.Parameters[0];
            var bodyExpr = Expression.Not(Expression.Invoke(expression, paramExpr));

            return Expression.Lambda<Func<T, bool>>(bodyExpr, paramExpr);
        }
    }
}
