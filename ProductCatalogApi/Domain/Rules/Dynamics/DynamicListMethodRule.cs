using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ProductCatalogApi.Domain.Rules.Dynamics
{
    public class DynamicListMethodRule<T> : DynamicCompositeRule<T>
    {
        private readonly string _parameterName;
        private readonly string _methodName;
        private readonly List<object> _values;

        public DynamicListMethodRule(string parameterName, string methodName, List<object> values)
        {
            _parameterName = parameterName;
            _methodName = methodName;
            _values = values;
        }

        public override Expression<Func<T, bool>> IsSatisfiedBy()
        {
            Expression parameter = Expression.Parameter(typeof(T), typeof(T).Name.ToLower().First().ToString());
            var property = _parameterName.Split('.').Aggregate(parameter, (leftProperty, rightProperty) => Expression.Property(leftProperty, rightProperty));
            var genericType = GetRealType(property.Type);
            var constantListType = typeof(List<>).MakeGenericType(new[] { genericType });
            var constantList = Activator.CreateInstance(constantListType) as IList;
            var constant = Expression.Constant(constantList);
            var methodInfo = constantListType.GetMethod(_methodName);
            var body = Expression.Call(constant, methodInfo, Expression.Convert(property, genericType));

            _values.ForEach(v => constantList.Add(Convert.ChangeType(v, genericType)));

            return Expression.Lambda<Func<T, bool>>(body, (ParameterExpression)parameter);
        }

        private static Type GetRealType(Type type)
        {
            var underlyingType = Nullable.GetUnderlyingType(type);
            return underlyingType ?? type;
        }
    }
}
