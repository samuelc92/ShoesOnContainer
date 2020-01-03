using System;
using System.Linq;
using System.Linq.Expressions;

namespace ProductCatalogApi.Domain.Rules.Dynamics
{
    public class DynamicRule<T> : DynamicCompositeRule<T>
    {
        private readonly string _parameterName;
        private readonly ExpressionType _expressionType;
        private readonly object _constantValue;

        public DynamicRule(string parameterName, ExpressionType expressionType, object constantValue)
        {
            _parameterName = parameterName;
            _expressionType = expressionType;
            _constantValue = constantValue;
        }

        public override Expression<Func<T, bool>> IsSatisfiedBy()
        {
            var parameter = Expression.Parameter(typeof(T), typeof(T).Name.ToLower().First().ToString());
            var property = Expression.Property(parameter, _parameterName);
            var constantValue = ConvertConstantValue(property.Type, _constantValue);
            var constant = Expression.Convert(Expression.Constant(constantValue), property.Type);

            var body = Expression.MakeBinary(_expressionType, property, constant);
            return Expression.Lambda<Func<T, bool>>(body, parameter);
                
            object ConvertConstantValue(Type type, object value)
            {
                var constantType = Nullable.GetUnderlyingType(type) ?? type;
                return Convert.ChangeType(value, constantType);
            }
        }

    }
}
