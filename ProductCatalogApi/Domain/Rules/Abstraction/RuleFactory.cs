using ProductCatalogApi.Domain.Rules.CatalogRules;

namespace ProductCatalogApi.Domain.Rules.Abstraction
{
    public class RuleFactory : IRuleFactory
    {
        public IGetCatalogBrandById GetGetCatalogBrandById => new GetCatalogBrandById();
    }
}
