using System;
using System.Linq.Expressions;

namespace ProductCatalogApi.Domain.Rules.CatalogRules
{
    public class GetCatalogBrandById : IGetCatalogBrandById
    {
        private int _id;

        public Expression<Func<CatalogBrand, bool>> IsSatisfiedBy()
        {
            return x => x.Id.Equals(_id);
        }

        public IGetCatalogBrandById SetId(int id)
        {
            _id = id;
            return this;
        }
    }
}
