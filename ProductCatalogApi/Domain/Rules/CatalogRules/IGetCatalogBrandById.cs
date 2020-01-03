using ProductCatalogApi.Domain.Rules.Abstraction;

namespace ProductCatalogApi.Domain.Rules.CatalogRules
{
    public interface IGetCatalogBrandById : IRule<CatalogBrand>
    {
        IGetCatalogBrandById SetId(int id);
    }
}
