using ProductCatalogApi.Domain.Rules.CatalogRules;

namespace ProductCatalogApi.Domain.Rules.Abstraction
{
    public interface IRuleFactory
    {
        IGetCatalogBrandById GetGetCatalogBrandById { get; }
    }
}
