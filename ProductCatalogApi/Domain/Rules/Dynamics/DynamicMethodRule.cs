using System;
using System.Linq;
using System.Linq.Expressions;

namespace ProductCatalogApi.Domain.Rules.Dynamics
{
    public class DynamicMethodRule<T> : DynamicCompositeRule<T>
    {
        private readonly string _parameterName;
        private readonly string _methodName;
        private readonly object _constantValue;

        public DynamicMethodRule(string parameterName, string methodName, object constantValue)
        {
            _parameterName = parameterName;
            _methodName = methodName;
            _constantValue = constantValue;
        }

        public override Expression<Func<T, bool>> IsSatisfiedBy()
        {
            Expression parameter = Expression.Parameter(typeof(T), typeof(T).Name.ToLower().First().ToString());
            var property = _parameterName.Split('.')
                                        .Aggregate(parameter, (leftProperty, rightProperty) => 
                                         Expression.Property(leftProperty, rightProperty));
            //var property = Expression.Property(parameter, _parameterName);
            var constant = Expression.Convert(Expression.Constant(_constantValue), property.Type);
            var methodInfo = typeof(string).GetMethod(_methodName, new[] { constant.Type });
            var body = Expression.Call(property, methodInfo, constant);
            return Expression.Lambda<Func<T, bool>>(body, (ParameterExpression)parameter);
        }
    }
}
