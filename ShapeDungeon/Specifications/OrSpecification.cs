using System.Linq.Expressions;

namespace ShapeDungeon.Specifications
{
    public class OrSpecification<T> : Specification<T>
    {
        private readonly Specification<T> _left;
        private readonly Specification<T> _right;

        public OrSpecification(
            Specification<T> left, 
            Specification<T> right)
        {
            _left = left ?? throw new ArgumentNullException(nameof(left));
            _right = right ?? throw new ArgumentNullException(nameof(right));
        }

        public override bool IsSatisfiedBy(T item)
            => _left.IsSatisfiedBy(item) 
            || _right.IsSatisfiedBy(item);

        public override Expression<Func<T, bool>> ToExpression()
        {
            var leftExpression = _left.ToExpression();
            var rightExpression = _right.ToExpression();

            var paramExpr = Expression.Parameter(typeof(T), "item");
            var bodyExpr = Expression.OrElse(
                leftExpression.Body,
                Expression.Invoke(rightExpression, paramExpr)
            );

            return Expression.Lambda<Func<T, bool>>(bodyExpr, paramExpr);
        }
    }
}
