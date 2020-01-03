using System;
using System.Linq.Expressions;

namespace ProductCatalogApi.Domain.Rules.Abstraction
{
    public interface IRule<T>
    {
        Expression<Func<T, bool>> IsSatisfiedBy();
    }
}
