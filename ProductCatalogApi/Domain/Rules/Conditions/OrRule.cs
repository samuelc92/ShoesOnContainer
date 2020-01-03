using ProductCatalogApi.Domain.Rules.Abstraction;
using System;
using System.Linq.Expressions;

namespace ProductCatalogApi.Domain.Rules.Conditions
{
    public class OrRule<T> : IRule<T>
    {
        public IRule<T> _left { get; set; }
        public IRule<T> _right { get; set; }

        public OrRule(IRule<T> left, IRule<T> right)
        {
            _left = left;
            _right = right;
        }

        public Expression<Func<T, bool>> IsSatisfiedBy()
        {
            var parameters = _left.IsSatisfiedBy().Parameters;
            var left = Expression.Invoke(_left.IsSatisfiedBy(), parameters);
            var right = Expression.Invoke(_right.IsSatisfiedBy(), parameters);

            var expression = Expression.OrElse(left, right);
            return Expression.Lambda<Func<T, bool>>(expression, parameters);
        }
    }
}
