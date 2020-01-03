using ProductCatalogApi.Domain.Rules.Abstraction;

namespace ProductCatalogApi.Domain.Rules.Dynamics
{
    public interface IDynamicCompositeRule<T> : IRule<T>
    {
        IRule<T> Equals(string parameter, object value);
        IRule<T> InRange(string paramter, object value, object valueTo);
    }
}
