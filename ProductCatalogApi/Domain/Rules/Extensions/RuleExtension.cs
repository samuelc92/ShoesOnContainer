using ProductCatalogApi.Domain.Rules.Abstraction;
using ProductCatalogApi.Domain.Rules.Conditions;

namespace ProductCatalogApi.Domain.Rules.Extensions
{
    public static class RuleExtension
    {
        public static IRule<T> Or<T>(this IRule<T> left, IRule<T> right)
        {
            return new OrRule<T>(left, right);
        }

        public static IRule<T> And<T>(this IRule<T> left, IRule<T> right)
        {
            return new AndRule<T>(left, right);
        }
    }
}
