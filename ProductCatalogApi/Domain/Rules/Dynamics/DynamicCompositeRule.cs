using ProductCatalogApi.Domain.Rules.Abstraction;
using ProductCatalogApi.Domain.Rules.Extensions;
using System;
using System.Linq.Expressions;

namespace ProductCatalogApi.Domain.Rules.Dynamics
{
    public abstract class DynamicCompositeRule<T> : IDynamicCompositeRule<T>
    {
        public abstract Expression<Func<T, bool>> IsSatisfiedBy();

        public IRule<T> Equals(string parameter, object value)
        {
            return new DynamicRule<T>(parameter, ExpressionType.Equal, value);
        }

        public IRule<T> LessThanOrEqual(string parameter, object value)
        {
            return new DynamicRule<T>(parameter, ExpressionType.LessThanOrEqual, value);
        }

        public IRule<T> InRange(string paramter, object value, object valueTo)
        {
            return new DynamicRule<T>(paramter, ExpressionType.GreaterThanOrEqual, value).And(LessThanOrEqual(paramter, valueTo));
        }

        public IRule<T> Contains(string parameter, object value)
        {
            return new DynamicMethodRule<T>(parameter, nameof(string.Contains), value);
        }

        protected Expression<Func<T, bool>> True()
        {
            return entity => true;
        }
    }
}
